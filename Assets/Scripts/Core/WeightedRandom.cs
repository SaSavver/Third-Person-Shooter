using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeightedRandom<T>
{
    public T GetRandomWeightItems(List<RandomItemContainer> items)
    {
        var weightSum = items.Sum(x => x.Weight);
        var rndWeight = UnityEngine.Random.Range(0f, weightSum);
        var current = 0f;

        for(int i = 0; i < items.Count; i++)
        {
            current += items[i].Weight;
            if (current >= rndWeight)
                return items[i].Item;
        }

        throw new ArgumentException("No way!");
    }

    public struct RandomItemContainer
    {
        public T Item;
        public float Weight;
    }
}
