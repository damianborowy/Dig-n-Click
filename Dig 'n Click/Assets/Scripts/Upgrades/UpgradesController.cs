using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesController : MonoBehaviour
{
    public static UpgradesController Instance;
    public Dictionary<Upgrade, int> UpgradesDictionary = new Dictionary<Upgrade, int>();
    public Dictionary<Upgrade, Dictionary<int, double>> UpgradesMultipliers;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            CreateUpgradeMultipliersDictionary();
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

    private void CreateUpgradeMultipliersDictionary()
    {
        UpgradesMultipliers = new Dictionary<Upgrade, Dictionary<int, double>>
        {
            {Upgrade.Upgrade1, new Dictionary<int, double>
            {
                {25, 3},
                {50, 2},
                {100, 10}
            }},
            {Upgrade.Upgrade2, new Dictionary<int, double>
            {
                {20, 1}
            }},
            {Upgrade.Upgrade3, new Dictionary<int, double>
            {
                {25, 5},
                {50, 10},
                {100, 10}
            }},
            {Upgrade.Upgrade4, new Dictionary<int, double>
            {
                {25, 4},
                {50, 8},
                {100, 8}
            }},
            {Upgrade.Upgrade5, new Dictionary<int, double>
            {
                {25, 2},
                {50, 2},
                {100, 2}
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