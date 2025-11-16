using System.Collections.Generic;

namespace Utility.ExtensionMethods
{
    public static class LinqExtensions
    {
        public static T MinBy<T>(this IEnumerable<T> source, System.Func<T, float> selector)
        {
            T minItem = default;
            float minValue = float.MaxValue;
            foreach (var item in source)
            {
                float value = selector(item);
                if (value < minValue)
                {
                    minValue = value;
                    minItem = item;
                }
            }
            return minItem;
        }
        
        public static T MaxBy<T>(this IEnumerable<T> source, System.Func<T, float> selector)
        {
            T maxItem = default;
            float maxValue = float.MinValue;
            foreach (var item in source)
            {
                float value = selector(item);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxItem = item;
                }
            }
            return maxItem;
        }
    }
}