using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesController : MonoBehaviour
{
    public static UpgradesController Instance;
    public Dictionary<Upgrade, int> UpgradesDictionary;
    public Dictionary<Upgrade, Dictionary<int, double>> UpgradesMultipliers;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        UpgradesDictionary[upgrade]++;
        GameController.Instance.ToggleUpgradeSlots();
    }

    public void OverrideUpgrades()
    {
        UpgradesDictionary = null;
        CreateEmptyUpgradesDictionary();
    }

    public void CreateEmptyUpgradesDictionary()
    {
        if (UpgradesDictionary != null) return;

        UpgradesDictionary = new Dictionary<Upgrade, int>();
        Upgrade upgrade = Upgrade.Upgrade1;

        try
        {
            while (upgrade.Next() != Upgrade.Upgrade1)
            {
                UpgradesDictionary.Add(upgrade, upgrade == Upgrade.Upgrade2 ? 1 : 0);
                upgrade = upgrade.Next();
            }
        }
        catch (ArgumentException) {}
    }

    public void CreateUpgradeMultipliersDictionary()
    {
        UpgradesMultipliers = new Dictionary<Upgrade, Dictionary<int, double>>
        {
            {Upgrade.Upgrade1, new Dictionary<int, double>
            {
                {25, 8},
                {50, 8},
                {100, 8},
                {150, 4},
                {200, 2.5}
            }},
            {Upgrade.Upgrade2, new Dictionary<int, double>
            {
                {20, 1}
            }},
            {Upgrade.Upgrade3, new Dictionary<int, double>
            {
                {25, 15},
                {50, 5},
                {100, 9},
                {150, 2.5}
            }},
            {Upgrade.Upgrade4, new Dictionary<int, double>
            {
                {25, 5},
                {50, 5},
                {100, 5}
            }},
            {Upgrade.Upgrade5, new Dictionary<int, double>
            {
                {25, 2},
                {50, 4},
                {100, 3}
            }},
            {Upgrade.Upgrade6, new Dictionary<int, double>
            {
                {25, 1.5},
                {50, 2},
                {100, 2}
            }},
            {Upgrade.Upgrade7, new Dictionary<int, double>
            {
                {25, 1.5},
                {50, 2},
                {100, 2}
            }}
        };
    }

    public static double CalculateMultiplier(Upgrade upgrade)
    {
        if (!Instance.UpgradesMultipliers.ContainsKey(upgrade)) return 1;
        
        double multiplier = 1;
        int level = Instance.UpgradesDictionary[upgrade];

        foreach (var i in Instance.UpgradesMultipliers[upgrade])
        {
            if (level >= i.Key)
                multiplier *= i.Value;
        }

        return multiplier;
    }

    public static double GetClosestMultiplier(Upgrade upgrade)
    {
        if (!Instance.UpgradesDictionary.ContainsKey(upgrade)) return 1;

        double multiplier = 1;
        int level = Instance.UpgradesDictionary[upgrade];

        foreach (var i in Instance.UpgradesMultipliers[upgrade])
        {
            if (level < i.Key)
                return i.Value;
        }

        return multiplier;
    }

    public static double CalculateUpgradeProductivity(Upgrade upgrade)
    {
        if (!Instance.UpgradesDictionary.ContainsKey(upgrade)) return 0;

        int level = Instance.UpgradesDictionary[upgrade];
        double multiplier = CalculateMultiplier(upgrade);
        double productivity = UpgradesConsts.GetUpgradeValues(upgrade).Productivity;

        productivity *= level * multiplier;

        return productivity;
    }
}