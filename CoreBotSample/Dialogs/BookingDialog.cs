// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.3

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace CoreBotSample.Dialogs
{
    public class BookingDialog : CancelAndHelpDialog
    {
        
        private const string ShortDescStepMsgText = "What is the issue?";

        public BookingDialog()
            : base(nameof(BookingDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
               
                ShortDescStepAsync,
                ConfirmStepAsync,
                FinalStepAsync,
            }));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

       

        private async Task<DialogTurnResult> ShortDescStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisIncidentDetails = (LuisIncidentDetails)stepContext.Options;

            //luisIncidentDetails.ShortDesc = (string)stepContext.Result;

            if (luisIncidentDetails.ShortDesc== null || IsAmbiguous(luisIncidentDetails.ShortDesc))
            {
                var promptMessage = MessageFactory.Text(ShortDescStepMsgText, ShortDescStepMsgText, InputHints.ExpectingInput);
                return await stepContext.BeginDialogAsync(nameof(DateResolverDialog), luisIncidentDetails.ShortDesc, cancellationToken);
            }

            return await stepContext.NextAsync(luisIncidentDetails.ShortDesc, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookingDetails = (LuisIncidentDetails)stepContext.Options;

            bookingDetails.ShortDesc = (string)stepContext.Result;

            var messageText = $"Please confirm, Are you sure you want to create incident  Is this correct?";
            var promptMessage = MessageFactory.Text(messageText, messageText, InputHints.ExpectingInput);

            return await stepContext.PromptAsync(nameof(ConfirmPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if ((bool)stepContext.Result)
            {
                var bookingDetails = (LuisIncidentDetails)stepContext.Options;

                return await stepContext.EndDialogAsync(bookingDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private static bool IsAmbiguous(string timex)
        {
            var timexProperty = new TimexProperty(timex);
            return !timexProperty.Types.Contains(Constants.TimexTypes.Definite);
        }
    }
}
