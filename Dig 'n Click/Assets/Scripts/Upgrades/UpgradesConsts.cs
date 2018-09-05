using System;
using UnityEngine;

public static class UpgradesConsts {

    // Clicker
    public const double Upgrade1BaseCost = 5;
    public const float Upgrade1CostMultiplier = 1.1f;
    public const double Upgrade1Productivity = 1;

    // Mining speed
    public const double Upgrade2BaseCost = 500;
    public const float Upgrade2CostMultiplier = 1.5f;
    public const double Upgrade2Productivity = 0.05;

    // Upgrade 3
    public const double Upgrade3BaseCost = 40;
    public const float Upgrade3CostMultiplier = 1.13f;
    public const double Upgrade3Productivity = 12;

    // Upgrade 4
    public const double Upgrade4BaseCost = 720;
    public const float Upgrade4CostMultiplier = 1.14f;
    public const double Upgrade4Productivity = 180;

    // Upgrade 5
    public const double Upgrade5BaseCost = 8640;
    public const float Upgrade5CostMultiplier = 1.13f;
    public const double Upgrade5Productivity = 1060;

    // Upgrade 6
    public const double Upgrade6BaseCost = 103680;
    public const float Upgrade6CostMultiplier = 1.12f;
    public const double Upgrade6Productivity = 4560;

    // Upgrade 7
    public const double Upgrade7BaseCost = 1244160;
    public const float Upgrade7CostMultiplier = 1.13f;
    public const double Upgrade7Productivity = 12960;

    public static double Exponent(double baseValue, float exponentialMultiplier, int level)
    {
        return Math.Floor(baseValue * Math.Pow(exponentialMultiplier, level));
    }

    public class UpgradeValues
    {
        public double BaseCost;
        public float CostMultiplier;
        public double Productivity;

        public UpgradeValues(double baseCost, float costMultiplier, double productivity)
        {
            BaseCost = baseCost;
            CostMultiplier = costMultiplier;
            Productivity = productivity;
        }
    }

    public static UpgradeValues GetUpgradeValues(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case Upgrade.Upgrade1:
                return new UpgradeValues(Upgrade1BaseCost, Upgrade1CostMultiplier, Upgrade1Productivity);
            case Upgrade.Upgrade2:
                return new UpgradeValues(Upgrade2BaseCost, Upgrade2CostMultiplier, Upgrade2Productivity);
            case Upgrade.Upgrade3:
                return new UpgradeValues(Upgrade3BaseCost, Upgrade3CostMultiplier, Upgrade3Productivity);
            case Upgrade.Upgrade4:
                return new UpgradeValues(Upgrade4BaseCost, Upgrade4CostMultiplier, Upgrade4Productivity);
            case Upgrade.Upgrade5:
                return new UpgradeValues(Upgrade5BaseCost, Upgrade5CostMultiplier, Upgrade5Productivity);
            case Upgrade.Upgrade6:
                return new UpgradeValues(Upgrade6BaseCost, Upgrade6CostMultiplier, Upgrade6Productivity);
            case Upgrade.Upgrade7:
                return new UpgradeValues(Upgrade7BaseCost, Upgrade7CostMultiplier, Upgrade7Productivity);
            case Upgrade.Upgrade8:
                break;
            case Upgrade.Upgrade9:
                break;
            case Upgrade.Upgrade10:
                break;
        }

        return null;
    }
}
