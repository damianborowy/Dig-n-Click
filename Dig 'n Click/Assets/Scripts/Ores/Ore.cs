using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ore
{
    public string Name;
    public float DropChance;
    public double Value;
    public int MinLevel;
    public int MaxLevel;
}

public class OreCompareByDropChance : IComparer<Ore>
{
    public int Compare(Ore x, Ore y)
    {
        if (x.DropChance > y.DropChance)
            return 1;
        else if (x.DropChance < y.DropChance)
            return -1;
        else
            return 0;
    }
}

public class OreCompareByValue : IComparer<Ore>
{
    public int Compare(Ore x, Ore y)
    {
        if (x.Value > y.Value)
            return 1;
        else if (x.Value < y.Value)
            return -1;
        else
            return 0;
    }
}