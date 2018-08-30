using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BasicEconomyValues
{
    // Ascending cost
    public const int BaseAscendCost = 5;
    public const int AscendBias = 0;
    public const float AscendExponentialMultiplier = 1.14f;

    // Rock values
    public const int BaseHealth = 10;
    public const int HealthBias = 29;
    public const float HealthExponentialMultiplier = 1.15f;
    public const float RockFallingTime = 0.3f;

    // Upgrade values
    public const int ToBeAdded = 0;

    // Earnings
    public const float BaseEarnings = 1.49f;

    public static double Exponent(int baseValue, int bias, float exponentialMultiplier, int level)
    {
        return Mathf.Floor(baseValue * Mathf.Pow(exponentialMultiplier, level) + bias);
    }

    public static double MoneyReward(int level)
    {
        return Mathf.Floor(BaseEarnings * level);
    }
}
