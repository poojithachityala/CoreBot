// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.


using System.Linq;

namespace CoreBotSample.CognitiveModels
{
    // Extends the partial FlightBooking class with methods and properties that simplify accessing entities in the luis results
    public partial class FlightBooking
    {
       
        // This value will be a TIMEX. And we are only interested in a Date so grab the first result and drop the Time part.
        // TIMEX is a format that represents DateTime expressions that include some ambiguity. e.g. missing a Year.
       
    }
}
