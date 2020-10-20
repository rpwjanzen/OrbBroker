namespace Trader.Util
{
    /// <summary>
    ///     Pattern matching interface for success of <see cref="IResult{TSuccess,TError}" />
    /// </summary>
    public interface ISuccess<out TSuccess>
    {
        TSuccess Value { get; }
    }
}
