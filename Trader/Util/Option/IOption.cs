using System;

namespace Trader.Util
{
    public interface IOption<out TSome>
    {
        TSome Some();

        TReturn Select<TReturn>(Func<TSome, TReturn> ofSome, Func<TReturn> ofNone);
        void Do(Action<TSome> ofSome, Action ofNone);
    }
}
