using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExensions
{
    public static T GetRandom<T>(this List<T> lst)
    {
        var rnd = new System.Random();
        return lst[rnd.Next(0, lst.Count - 1)];
    }

    public static T GetRandom<T>(this T[] array)
    {
        var rnd = new System.Random();
        return array[rnd.Next(0, array.Length - 1)];
    }
}
