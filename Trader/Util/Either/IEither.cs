using System;

namespace Trader
{
    /// <summary>
    ///     Either monad converted to non-functional C# idioms
    /// </summary>
    /// <example>
    /// IEither<string, bool> someEither = ...;
    /// switch(someEither)
    /// {
    ///     case ILeft<string> left: Console.WriteLine("Woo!"); break;
    ///     case IRight<bool> right: Console.WriteLine("Yay!"); break;
    /// }
    /// </example>
    public interface IEither<out TLeft, out TRight>
    {
        TReturn Select<TReturn>(Func<TLeft, TReturn> ofLeft, Func<TRight, TReturn> ofRight);
        void Do(Action<TLeft> ofLeft, Action<TRight> ofRight);

        TLeft Left();
        TRight Right();
    }
}
