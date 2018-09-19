using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RockValuesCalculator
{
    public static int GetRocksDestroyedByTime(int level, double damagePerSecond, double time)
    {
        double timeToDestroyRock = GetTimeToDestroyRock(level, damagePerSecond);
        return (int) (time / timeToDestroyRock);
    }

    private static double GetTimeToDestroyRock(int level, double damagePerSecond)
    {
        return BasicEconomyValues.RockFallingTime + GetRockHealth(level) / damagePerSecond;
    }

    private static double GetRockHealth(int level)
    {
        return BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias,
            BasicEconomyValues.HealthExponentialMultiplier, level);
    }
}