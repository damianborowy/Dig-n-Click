using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowIdleRewardWindow : MonoBehaviour
{
    public TextMeshProUGUI Text;

    private bool _isInitialized;

    private void Start()
    {
        if (!GameController.Instance.IsFirstLaunch())
        {
            CreateRewardWindow();
        }
        else
        {
            GameController.Instance.NotFirstLaunch();
            transform.localScale = Vector3.zero;
        }

        _isInitialized = true;
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus && _isInitialized)
            CreateRewardWindow();
    }

    private void CreateRewardWindow()
    {
        transform.localScale = Vector3.one;
        foreach (Transform child in transform)
        {
            if (child.CompareTag("ExitButton"))
                child.gameObject.GetComponent<Button>().interactable = true;
            if (child.CompareTag("Text"))
                Destroy(child.gameObject);
        }
        double idleTime = GameController.Instance.GetIdleTime();

        KeyValuePair<double, Dictionary<Ore, int>> rewardMoneyOres;
        if (idleTime > 0)
        {
            rewardMoneyOres = IdleReward(idleTime);
        }
        else
        {
            throw new NotSupportedException("Negative idle time");
        }

        double moneyReward = rewardMoneyOres.Key;
        Dictionary<Ore, int> oreReward = rewardMoneyOres.Value;

        Text.text = "<br><size=%27>While you were away<br>you mined:</size><br><br> ";
        if (moneyReward < 1 && oreReward.Count == 0)
            Text.text += "Nothing!<br> ";
        else
        {
            if (moneyReward >= 1)
                Text.text += "<color=#FFFF00><b>+$" + MoneyConverter.ConvertNumber(moneyReward) + "</b></color><br> ";

            if (oreReward.Count != 0)
                foreach (var element in oreReward)
                    Text.text += "<color=#" + ColorUtility.ToHtmlStringRGB(element.Key.DropTextColor) + ">+" + element.Value + " " + element.Key.Name + "</color><br> ";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public static KeyValuePair<double, Dictionary<Ore, int>> IdleReward(double idleTime)
    {
        var instance = GameController.Instance;
        int timeLimit = 43200;

        if (idleTime > timeLimit)
        {
            idleTime = timeLimit;
        }

        double maxRockHealth = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias,
            BasicEconomyValues.HealthExponentialMultiplier, instance.GetMaxLevel());
        double currentRockHealth = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth,
            BasicEconomyValues.HealthBias, BasicEconomyValues.HealthExponentialMultiplier, instance.GetLevel());

        instance.CalculateAutoStrength();
        double miningSpeed = instance.GetMiningSpeed();

        double avgTimeToDestroyMaxRock = maxRockHealth * miningSpeed / instance.GetAutoStrength() +
                                         BasicEconomyValues.RockFallingTime;
        double avgTimeToDestroyCurrentRock = currentRockHealth * miningSpeed / instance.GetAutoStrength() +
                                             BasicEconomyValues.RockFallingTime;

        int maxRocksDestroyed = Convert.ToInt32(idleTime / avgTimeToDestroyMaxRock);
        int currentRocksDestroyed = Convert.ToInt32(idleTime / avgTimeToDestroyCurrentRock);

        double moneyToBeAdded = BasicEconomyValues.MoneyReward(instance.GetMaxLevel()) * maxRocksDestroyed;
        instance.AddMoney(moneyToBeAdded);

        OreDropper dropperScript = GameObject.FindGameObjectWithTag("Dropper").GetComponent<OreDropper>();
        Dictionary<Ore, int> droppedOres = dropperScript.DropOre(currentRocksDestroyed);

        return new KeyValuePair<double, Dictionary<Ore, int>>(moneyToBeAdded, droppedOres);
    }
}