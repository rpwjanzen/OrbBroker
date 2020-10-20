namespace Trader.IO
{
    class ValidationError
    {
        public string FieldName { get; }
        public string Description { get; }
        public ValidationError(string fieldName, string description)
        {
            FieldName = fieldName;
            Description = description;
        }
    }

    delegate IResult<ValidatedBuyCommand, ValidationError[]> ValidateBuyCommand(UnvalidatedBuyCommand arg);
    delegate IResult<ValidatedCommandText, ValidationError[]> ValidateCommandText(UnvalidatedCommandText arg);
    class UnvalidatedCommandText { }
    class ValidatedCommandText { }

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
