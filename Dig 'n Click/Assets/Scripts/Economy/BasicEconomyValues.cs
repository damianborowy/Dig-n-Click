using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BasicEconomyValues
{
    // Ascending cost
    public const int BaseAscendCost = 4;
    public const int AscendBias = 6;
    public const float AscendExponentialMultiplier = 1.14f;

    // Rock values
    public const int BaseHealth = 30;
    public const int HealthBias = 15;
    public const float HealthExponentialMultiplier = 1.15f;
    public const float RockFallingTime = 0.45f;

    // Earnings
    public const float BaseEarnings = 2;

    public static double Exponent(int baseValue, int bias, float exponentialMultiplier, int level)
    {
        return Mathf.Ceil(baseValue * Mathf.Pow(exponentialMultiplier, level) + bias);
    }

    public static double MoneyReward(int level)
    {
        return Mathf.Ceil(BaseEarnings * level);
    }
}