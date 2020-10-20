namespace Trader.Util
{
    interface ISome<out TSome>
    {
        TSome Value { get; }
    }
}
