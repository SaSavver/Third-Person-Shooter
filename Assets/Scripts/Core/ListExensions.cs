using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExensions
{
    public static T GetRandom<T>(this List<T> lst)
    {
        return lst[Random.Range(0, lst.Count)];
    }

    public static T GetRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
