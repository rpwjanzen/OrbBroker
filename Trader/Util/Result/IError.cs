namespace Trader.Util
{
    /// <summary>
    ///     Pattern matching interface for error of <see cref="IResult{TSuccess,TError}" />
    /// </summary>
    public interface IError<out TError>
    {
        TError Value { get; }
    }
}
