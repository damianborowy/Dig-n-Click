using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ore
{
    public string Name;
    public Sprite OreSprite;
    public Color DropTextColor;
    public float DropWeight;
    public double Value;
    public int MinLevel;
    public int MaxLevel;
}

[Serializable]
public class OreCompareByValue : IComparer<Ore>
{
    //Comparation for decreasing value order
    public int Compare(Ore x, Ore y)
    {
        if (x.Value < y.Value)
            return 1;
        if (x.Value > y.Value)
            return -1;
        return 0;
    }
}