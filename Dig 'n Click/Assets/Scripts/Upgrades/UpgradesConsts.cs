using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradesConsts {

    // Clicker
    public const double Upgrade1BaseCost = 3.738f;
    public const float Upgrade1CostMultiplier = 1.07f;
    public const double Upgrade1Productivity = 1;

    // AutoMiner speed
    public const double Upgrade2BaseCost = 60;
    public const float Upgrade2CostMultiplier = 1.15f;
    public const double Upgrade2Productivity = 0.05;

    // Upgrade 3
    public const double Upgrade3BaseCost = 720;
    public const float Upgrade3CostMultiplier = 1.14f;
    public const double Upgrade3Productivity = 90;

    // Upgrade 4
    public const double Upgrade4BaseCost = 8640;
    public const float Upgrade4CostMultiplier = 1.13f;
    public const double Upgrade4Productivity = 360;

    // Upgrade 5
    public const double Upgrade5BaseCost = 103680;
    public const float Upgrade5CostMultiplier = 1.12f;
    public const double Upgrade5Productivity = 2160;

    // Upgrade 6
    public const double Upgrade6BaseCost = 1244160;
    public const float Upgrade6CostMultiplier = 1.12f;
    public const double Upgrade6Productivity = 12960;

    public static double Exponent(int baseValue, float exponentialMultiplier, int level)
    {
        return Mathf.Floor(baseValue * Mathf.Pow(exponentialMultiplier, level));
    }
}
