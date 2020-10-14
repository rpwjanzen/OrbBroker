namespace Trader
{
    /// <summary>
    ///     Static helper class for Either
    /// </summary>
    public static class Either
    {
        private struct LeftImpl<TLeft, TRight> : IEither<TLeft, TRight>, ILeft<TLeft>
        {
            public TLeft Value { get; }
            public TLeft Left() => Value;
            public TRight Right() => throw new System.InvalidOperationException();

            public LeftImpl(TLeft value)
            {
                Value = value;
            }

            public TReturn Select<TReturn>(System.Func<TLeft, TReturn> ofLeft, System.Func<TRight, TReturn> ofRight)
            {
                if (ofLeft == null)
                    throw new System.ArgumentNullException(nameof(ofLeft));

                return ofLeft(Value);
            }

            public void Do(System.Action<TLeft> ofLeft, System.Action<TRight> ofRight)
            {
                if (ofLeft == null)
                    throw new System.ArgumentNullException(nameof(ofLeft));

                ofLeft(Value);
            }
        }

        private struct RightImpl<TLeft, TRight> : IEither<TLeft, TRight>, IRight<TRight>
        {
            public TRight Value { get; }
            public TLeft Left() => throw new System.InvalidOperationException();
            public TRight Right() => Value;

            public RightImpl(TRight value)
            {
                Value = value;
            }

            public TReturn Select<TReturn>(System.Func<TLeft, TReturn> ofLeft, System.Func<TRight, TReturn> ofRight)
            {
                if (ofRight == null)
                    throw new System.ArgumentNullException(nameof(ofRight));

                return ofRight(Value);
            }

            public void Do(System.Action<TLeft> ofLeft, System.Action<TRight> ofRight)
            {
                if (ofRight == null)
                    throw new System.ArgumentNullException(nameof(ofRight));

                ofRight(Value);
            }
        }

        /// <summary>
        ///     Create an Either with Left value
        /// </summary>
        public static IEither<TLeft, TRight> Left<TLeft, TRight>(TLeft value)
        {
            return new LeftImpl<TLeft, TRight>(value);
        }

        /// <summary>
        ///     Create an Either with Right value
        /// </summary>
        public static IEither<TLeft, TRight> Right<TLeft, TRight>(TRight value)
        {
            return new RightImpl<TLeft, TRight>(value);
        }
    }
}
