using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWeightedListEntry
{
    public int Weight { get; }
}
public struct WeightedList<T> where T : IWeightedListEntry
{
    private readonly struct WeightedEntry
    {
        public readonly int CumulativeWeight;
        public readonly T Item;
        public WeightedEntry(int CumulativeWeight, T Item)
        {
            this.CumulativeWeight = CumulativeWeight;
            this.Item = Item;
        }
    }
    public readonly int TotalWeight;
    private readonly WeightedEntry[] array;
    public WeightedList(IList<T> itemsToWeight)
    {
        array = new WeightedEntry[itemsToWeight.Count];        
        TotalWeight = 0;
        for (int i = 0; i < itemsToWeight.Count; i++)
        {
            var item = itemsToWeight[i];
            TotalWeight += Mathf.Max(item.Weight,0);
            array[i] = new WeightedEntry(
                TotalWeight,
                item
            );
        }
    }
    public T GetRandom(int randomInt)
    {
        randomInt %= TotalWeight;
        int low = 0;
        int high = array.Length - 1;
        while (low < high)
        {
            //this generates the midpoint of the current range (from low to high).
            int mid = low + (high - low) / 2;
            if (randomInt < array[mid].CumulativeWeight) 
                // if the randomInt is less than the cumulative weight at mid, then the desired item is in the lower half of the range (from low to mid).
            {
                high = mid;
            }
            else
            {
                //discard the lower half of the range (from low to mid) and continue searching in the upper half (from mid + 1 to high).
                low = mid + 1;
            }
        }
        return array[low].Item;
    }
    #if UNITY_EDITOR
    public void DumpContents(Func<T,string> stringConverter)
    {
        int prevWeight = 0;
        for(int i = 0; i < array.Length; i++) {
            Debug.Log($"Item #{i}: {stringConverter(array[i].Item)}, weight {array[i].CumulativeWeight - prevWeight}");
            prevWeight = array[i].CumulativeWeight;
        }
    }
    #endif
}
