using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowIdleRewardWindow : MonoBehaviour
{
    public string FontName;
    public int FontSize;

    private GameObject _textGameObject;
    private bool _isInitialized;

    private void Start()
    {
        if (!GameController.Instance.IsFirstLaunch())
        {
            CreateRewardWindow();
        }
        else
            transform.localScale = Vector3.zero;

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

        _textGameObject = new GameObject("Text");
        _textGameObject.tag = "Text";
        RectTransform textRectTransform = _textGameObject.AddComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(textRectTransform.rect.width, FontSize);
        Text text = _textGameObject.AddComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont(FontName, FontSize);
        text.fontSize = FontSize;
        text.alignment = TextAnchor.MiddleCenter;

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

        if (moneyReward < 1 && oreReward.Count == 0)
        {
            GameObject instantiatedGameObject = Instantiate(_textGameObject, transform);
            instantiatedGameObject.GetComponent<Text>().text = "Nothing!";
        }
        else
        {
            if (moneyReward >= 1)
            {
                GameObject instantiatedMoneyGameObject = Instantiate(_textGameObject, transform);
                instantiatedMoneyGameObject.GetComponent<Text>().text =
                    "+ $" + MoneyConverter.ConvertNumber(moneyReward);
            }

            if (oreReward.Count != 0)
                foreach (var element in oreReward)
                {
                    GameObject instantiatedGameObject = Instantiate(_textGameObject, transform);
                    instantiatedGameObject.GetComponent<Text>().text = "+" + element.Value + " " + element.Key.Name;
                }
        }

        Instantiate(_textGameObject, transform); //empty filler
    }

    public static KeyValuePair<double, Dictionary<Ore, int>> IdleReward(double idleTime)
    {
        var instance = GameController.Instance;

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