using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ore
{
    public string Name;
    public Color DropTextColor;
    public float DropWeight;
    public double Value;
    public int MinLevel;
    public int MaxLevel;
}

public class OreCompareByDropChance : IComparer<Ore>
{
    public int Compare(Ore x, Ore y)
    {
        if (x.DropWeight > y.DropWeight)
            return 1;
        if (x.DropWeight < y.DropWeight)
            return -1;
        return 0;
    }
}

public class OreCompareByValue : IComparer<Ore>
{
    public int Compare(Ore x, Ore y)
    {
        if (x.Value > y.Value)
            return 1;
        if (x.Value < y.Value)
            return -1;
        return 0;
    }
}