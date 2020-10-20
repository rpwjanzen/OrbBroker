namespace Trader.Util
{
    class NotEmpty<T>
    {
        public T Item { get; }
        public NotEmpty(T t) => Item = t;

        public static implicit operator T(NotEmpty<T> n) => n.Item;
    }

    static class NotEmpty
    {
        public static IOption<NotEmpty<string>> CreateNotEmpty(string text)
        {
            if (text != "")
            {
                NotEmpty<string> notEmpty = new NotEmpty<string>(text);
                return Option.Some(notEmpty);
            }
            return Option.None<NotEmpty<string>>();
        }
    }
}
