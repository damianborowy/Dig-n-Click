using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleEarnings
{
    public static void IdleReward(int idleTime)
    {
        var instance = GameController.Instance;

        double maxRockHealth = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias, BasicEconomyValues.HealthExponentialMultiplier, instance.GetMaxLevel());
        double currentRockHealth = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias, BasicEconomyValues.HealthExponentialMultiplier, instance.GetLevel());

        instance.CalculateAutoStrength();
        double avgTimeToDestroyMaxRock = maxRockHealth / instance.GetAutoStrength() + BasicEconomyValues.RockFallingTime;
        double avgTimeToDestroyCurrentRock = currentRockHealth / instance.GetAutoStrength() + BasicEconomyValues.RockFallingTime;

        int maxRocksDestroyed = Convert.ToInt32(idleTime / avgTimeToDestroyMaxRock);
        int currentRocksDestroyed = Convert.ToInt32(idleTime / avgTimeToDestroyCurrentRock);

        instance.AddMoney(BasicEconomyValues.MoneyReward(instance.GetMaxLevel()) * maxRocksDestroyed);

        OreDropper dropperScript = GameObject.FindGameObjectWithTag("Dropper").GetComponent<OreDropper>();
        dropperScript.DropOre(currentRocksDestroyed);
    }
}
