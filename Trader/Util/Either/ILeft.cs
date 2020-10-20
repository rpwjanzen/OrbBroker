namespace Trader
{
    /// <summary>
    ///     Pattern matching interface for LHS of <see cref="IEither{TLeft,TRight}" />
    /// </summary>
    public interface ILeft<out TLeft>
    {
        TLeft Value { get; }
    }
}
