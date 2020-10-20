using System;

namespace Trader
{
    interface IResult<out TSuccess, out TError>
    {
        TReturn Select<TReturn>(Func<TSuccess, TReturn> ofSuccess, Func<TError, TReturn> ofError);
        void Do(Action<TSuccess> ofSuccess, Action<TError> ofError);

        TSuccess Success();
        TError Error();
    }
}
