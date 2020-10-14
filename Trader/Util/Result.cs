namespace Trader
{
    abstract class Result
    {
        public static Success<T> CreateSuccess<T>(T t)
        {
            return new Success<T>() { Value = t };
        }

        public static Failure CreateFailure(string error)
        {
            return new Failure() { Error = error };
        }

        public sealed class Success<T> : Result
        {
            public T Value;
        }

        public sealed class Failure: Result
        {
            public string Error;
        }

        public TResult Match<T, TResult>(System.Func<Success<T>, TResult> onSuccess, System.Func<Failure, TResult> onFailure)
        {
            if (this is Success<T>)
            {
                return onSuccess(this as Success<T>);
            }
            return onFailure(this as Failure);
        }
    }
}
