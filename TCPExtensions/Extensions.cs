using System;
using System.Collections.Generic;

namespace TCPExtensions;

public static class Extensions
{
    public static List<T> Filter<T>(this IList<T> records, Func<T, bool> func)
    {
        List<T> filterList = [];
        foreach (T record in records)
        {
            if (func(record))
            {
                filterList.Add(record);
            }
        }
        return filterList;
    }
    public static List<T> Filter<T>(this List<T> records, Func<T, bool> func)
    {
        List<T> filterList = [];
        foreach (T record in records)
        {
            if (func(record))
            {
                filterList.Add(record);
            }
        }
        return filterList;
    }

}
