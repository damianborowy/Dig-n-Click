using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowIdleRewardWindow : MonoBehaviour
{
    public string FontName;
    public int FontSize;

    private GameObject textGameObject;

    private void Start()
    {
        if (!GameController.Instance.IsFirstLaunch())
        {
            GameObject fadingTextSpawn = GameObject.FindWithTag("FadingTextSpawn");

            textGameObject = new GameObject("MinedOreText");
            RectTransform textRectTransform = textGameObject.AddComponent<RectTransform>();
            textRectTransform.sizeDelta = new Vector2(textRectTransform.rect.width, FontSize);
            Text text = textGameObject.AddComponent<Text>();
            text.font = Font.CreateDynamicFontFromOSFont(FontName, FontSize);
            text.fontSize = FontSize;
            text.alignment = TextAnchor.MiddleCenter;

            int idleTime = (int) (DateTime.UtcNow - GameController.EpochTimeStart).TotalSeconds -
                           GameController.Instance.GetIdleTime();

            double moneyReward = 0;

            if (idleTime > 0)
            {
                moneyReward = IdleReward(idleTime);
            }

            if (fadingTextSpawn.transform.childCount == 0 && moneyReward == 0)
            {
                GameObject instantiatedGameObject = Instantiate(textGameObject, transform);
                instantiatedGameObject.GetComponent<Text>().text = "Nothing!";
            }
            else
            {
                GameObject instantiatedMoneyGameObject = Instantiate(textGameObject, transform);
                instantiatedMoneyGameObject.GetComponent<Text>().text =
                    "+ $" + MoneyConverter.ConvertNumber(moneyReward);

                foreach (Transform child in fadingTextSpawn.transform)
                {
                    GameObject instantiatedGameObject = Instantiate(textGameObject, transform);
                    instantiatedGameObject.GetComponent<Text>().text = child.gameObject.GetComponent<Text>().text;
                }
            }

            Instantiate(textGameObject, transform); //empty filler
        }
        else
        {
            Destroy(GameObject.FindGameObjectWithTag("IdleRewardWindow"));
        }
    }

    public void DestroyIdleRewardWindow()
    {
        Destroy(gameObject);
    }

    public static double IdleReward(int idleTime)
    {
        var instance = GameController.Instance;
        var maxlevel = instance.GetMaxLevel();

        double maxRockHealth = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias, BasicEconomyValues.HealthExponentialMultiplier, instance.GetMaxLevel());
        double currentRockHealth = BasicEconomyValues.Exponent(BasicEconomyValues.BaseHealth, BasicEconomyValues.HealthBias, BasicEconomyValues.HealthExponentialMultiplier, instance.GetLevel());

        instance.CalculateAutoStrength();
        double avgTimeToDestroyMaxRock = maxRockHealth / instance.GetAutoStrength() + BasicEconomyValues.RockFallingTime;
        double avgTimeToDestroyCurrentRock = currentRockHealth / instance.GetAutoStrength() + BasicEconomyValues.RockFallingTime;

        int maxRocksDestroyed = Convert.ToInt32(idleTime / avgTimeToDestroyMaxRock);
        int currentRocksDestroyed = Convert.ToInt32(idleTime / avgTimeToDestroyCurrentRock);

        double moneyToBeAdded = BasicEconomyValues.MoneyReward(instance.GetMaxLevel()) * maxRocksDestroyed;

        instance.AddMoney(moneyToBeAdded);

        OreDropper dropperScript = GameObject.FindGameObjectWithTag("Dropper").GetComponent<OreDropper>();
        dropperScript.DropOre(currentRocksDestroyed);

        return moneyToBeAdded;
    }
}