using System;

namespace Trader.Util
{
    static class Result
    {
        private struct SuccessImpl<TSuccess, TError> : IResult<TSuccess, TError>, ISuccess<TSuccess>
        {
            public TSuccess Value { get; }
            public TError Error() => throw new InvalidOperationException();
            public TSuccess Success() => Value;

            public SuccessImpl(TSuccess value)
            {
                Value = value;
            }

            public void Do(Action<TSuccess> ofSuccess, Action<TError> ofError) => ofSuccess(Value);
            public TReturn Select<TReturn>(Func<TSuccess, TReturn> ofSuccess, Func<TError, TReturn> ofError) => ofSuccess(Value);
        }

        private struct ErrorImpl<TSuccess, TError> : IResult<TSuccess, TError>, IError<TError>
        {
            public TError Value { get; }
            public TError Error() => Value;
            public TSuccess Success() => throw new InvalidOperationException();

            public ErrorImpl(TError value)
            {
                Value = value;
            }

            public void Do(Action<TSuccess> ofSuccess, Action<TError> ofError) => ofError(Value);

            public TReturn Select<TReturn>(Func<TSuccess, TReturn> ofSuccess, Func<TError, TReturn> ofError)
                => ofError(Value);
        }

        public static IResult<TSuccess, TError> Success<TSuccess, TError>(TSuccess value) => new SuccessImpl<TSuccess, TError>(value);
        public static IResult<TSuccess, TError> Error<TSuccess, TError>(TError value) => new ErrorImpl<TSuccess, TError>(value);
    }
}
