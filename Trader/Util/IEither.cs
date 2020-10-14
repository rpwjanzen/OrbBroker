using System;
using System.Collections.Generic;
using System.Text;

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
        TReturn Select<TReturn>(System.Func<TLeft, TReturn> ofLeft, System.Func<TRight, TReturn> ofRight);

        void Do(System.Action<TLeft> ofLeft, System.Action<TRight> ofRight);

        /// <summary>
        ///     Provides the left value
        /// </summary>
        /// <exception cref="InvalidOperationException">if this is a right value</exception>
        TLeft Left();

        /// <summary>
        ///     Provides the right value
        /// </summary>
        /// <exception cref="InvalidOperationException">if this is a left value</exception>
        TRight Right();
    }
}
