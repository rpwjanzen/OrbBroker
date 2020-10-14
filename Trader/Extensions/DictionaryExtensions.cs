using System.Collections.Generic;


namespace Trader
{
    static class DictionaryExtensions
    {
        public static void AddOrUpdate<T, TKey, TValue>(this T d, TKey key, TValue addValue, System.Func<TValue, TValue> onUpdate) where T: Dictionary<TKey, TValue> {
            if (d.ContainsKey(key))
            {
                d[key] = onUpdate(d[key]);
            } else
            {
                d[key] = addValue;
            }
        }
    }
}
