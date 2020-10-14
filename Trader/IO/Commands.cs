using System;
using System.Collections.Generic;
using System.Text;

namespace Trader.IO
{
    static class Types
    {
        public static Func<UnvalidatedBuyCommand, ValidatedBuyCommand> ValidateBuyCommand;
    }

    class CommandText
    {
        public static void FromText(string text)
        {

        }
    }

    class UnparsedCommand { }
    class UnvalidatedBuyCommand { }
    class UnvalidatedSellCommand { }
    class UnvalidatedTravelCommand { }
    class ValidatedBuyCommand { }

    class ValidatedSellCommand { }
    class ValidatedTravelCommand { }
    class QuitCommand { }
    class SaveCommand { }
}
