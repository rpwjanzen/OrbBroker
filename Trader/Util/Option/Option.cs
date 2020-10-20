using System;

namespace Trader.Util
{
    static class Option
    {
        private struct SomeImpl<TSome> : IOption<TSome>, ISome<TSome>
        {
            public TSome Value { get; }
            public TSome Some() => Value;

            public SomeImpl(TSome some) => Value = some;
            public void Do(Action<TSome> ofSome, Action ofNone) => ofSome(Value);
            public TReturn Select<TReturn>(Func<TSome, TReturn> ofSome, Func<TReturn> ofNone) => ofSome(Value);
        }

        private struct NoneImpl<TSome> : IOption<TSome>, INone<TSome>
        {
            public TSome Some() => throw new InvalidOperationException();

            public void Do(Action<TSome> ofSome, Action ofNone) => ofNone();
            public TReturn Select<TReturn>(Func<TSome, TReturn> ofSome, Func<TReturn> ofNone) => ofNone();
        }

        public static IOption<TSome> Some<TSome>(TSome some) => new SomeImpl<TSome>(some);
        public static IOption<TSome> None<TSome>() => new NoneImpl<TSome>();
    }
}
