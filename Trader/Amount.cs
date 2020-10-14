namespace Trader
{
    abstract class Amount
    {
        private static readonly Amount MaxInstance = new Max();

        public static Result ReadAmount(string s)
        {
            if (s == "max" || s == "m")
            {
                return Result.CreateSuccess(Amount.MaxInstance);
            }

            if (!int.TryParse(s, out var number))
            {
                return Result.CreateFailure("Amount must be a number.");
            }

            if (number <= 0)
            {
                return Result.CreateFailure("Amount must be positive.");
            }

            return Result.CreateSuccess(new Number() { Count = number });
        }

        public TResult Match<TResult>(System.Func<TResult> onMax, System.Func<Number, TResult> onAmount)
        {
            if (this is Max)
            {
                return onMax();
            }
            return onAmount(this as Number);
        }

        public sealed class Max : Amount { }
        public sealed class Number : Amount
        {
            public int Count;
        }
    }
}
