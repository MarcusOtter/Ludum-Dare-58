using System;
using System.Collections.Generic;
using System.Linq;

public static class ArrayHelpers
{
    /// Returns a random item, where probability scales with weight
    public static T PickWeighted<T>(this IEnumerable<T> items, Func<T, int> weightSelector)
    {
        var enumeratedItems = items as T[] ?? items.ToArray();
        var total = enumeratedItems.Sum(weightSelector);
        var roll = UnityEngine.Random.Range(0, total);
        
        foreach (var item in enumeratedItems)
        {
            var weight = weightSelector(item);
            if (roll < weight)
            {
                return item;
            }
            
            roll -= weight;
        }
        
        return default;
    }
}
