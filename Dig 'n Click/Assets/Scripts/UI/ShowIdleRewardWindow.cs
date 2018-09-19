using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowIdleRewardWindow : MonoBehaviour
{
    public const int TimeLimit = 12 * 60 * 60;
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
                    Text.text += "<color=#" + ColorUtility.ToHtmlStringRGB(element.Key.DropTextColor) + ">+" +
                                 element.Value + " " + element.Key.Name + "</color><br> ";
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    public static KeyValuePair<double, Dictionary<Ore, int>> IdleReward(double idleTime)
    {
        if (idleTime > TimeLimit)
        {
            idleTime = TimeLimit;
        }

        int currentRocksDestroyed = GetCurrentRocksDestroyed(idleTime);
        int maxRocksDestroyed = GetMaxRocksDestroyed(idleTime);

        double moneyToBeAdded = GetMoneyToBeAdded(maxRocksDestroyed);
        GameController.Instance.AddMoney(moneyToBeAdded);

        Dictionary<Ore, int> droppedOres = DropOres(currentRocksDestroyed);

        return new KeyValuePair<double, Dictionary<Ore, int>>(moneyToBeAdded, droppedOres);
    }

    private static int GetCurrentRocksDestroyed(double idleTime)
    {
        int currentLevel = GameController.Instance.GetLevel();
        double damagePerSecond = GameController.Instance.GetMiningPower();
        return RockValuesCalculator.GetRocksDestroyedByTime(currentLevel, damagePerSecond, idleTime);
    }

    private static int GetMaxRocksDestroyed(double idleTime)
    {
        int maxLevel = GameController.Instance.GetMaxLevel();
        double damagePerSecond = GameController.Instance.GetMiningPower();
        return RockValuesCalculator.GetRocksDestroyedByTime(maxLevel, damagePerSecond, idleTime);
    }

    private static double GetMoneyToBeAdded(int rocksDestroyed)
    {
        int maxLevel = GameController.Instance.GetMaxLevel();
        return BasicEconomyValues.MoneyReward(maxLevel) * rocksDestroyed;
    }

    private static Dictionary<Ore, int> DropOres(int dropAttempts)
    {
        OreDropper dropper = GameObject.FindGameObjectWithTag("Dropper").GetComponent<OreDropper>();
        return dropper.DropOre(dropAttempts);
    }
}