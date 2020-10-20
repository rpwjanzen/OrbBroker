namespace Trader
{
    /// <summary>
    ///     Pattern matching interface for RHS of <see cref="IEither{TLeft,TRight}" />
    /// </summary>
    public interface IRight<out TRight>
    {
        TRight Value { get; }
    }
}
