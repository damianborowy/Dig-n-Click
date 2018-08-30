using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UpgradePanelHandler : MonoBehaviour
{
    public Upgrade Upgrade;
    public Text UpgradeText;
    public Text UpgradeLevel;
    public Slider MultiplyProgressSlider;

    private UpgradesConsts.UpgradeValues _upgradeValues;
    private double _nextUpgradeCost;
    private Color _full = new Color(255, 255, 255, 1);
    private Color _faded = new Color(255, 255, 255, 0.9f);
    private Button _upgradeButton;

    private void Awake()
    {
        _upgradeButton = gameObject.GetComponentInChildren<Button>();
    }

    private void Start()
    {
        _upgradeValues = UpgradesConsts.GetUpgradeValues(Upgrade);

        CalculateNextUpgradeCost();
        ToggleButtonFade();

        if (Upgrade.Equals(Upgrade.Upgrade1))
        {
            UpgradeText.text = "+ " + _upgradeValues.Productivity + " strength";
        }
        else if (Upgrade.Equals(Upgrade.Upgrade2))
        {
            UpgradeText.text = "- " + _upgradeValues.Productivity + " autominer speed";
        }
        else
        {
            UpgradeText.text = "+ " + _upgradeValues.Productivity + " autostrength";
        }
    }

    public void ToggleButtonFade()
    {
        if (GameController.Instance.GetMoney() >= _nextUpgradeCost)
        {
            _upgradeButton.image.color = _full;
            _upgradeButton.interactable = true;
        }
        else
        {
            _upgradeButton.image.color = _faded;
            _upgradeButton.interactable = false;
        }

        if (Upgrade.Equals(Upgrade.Upgrade2) && UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade2] >= 19)
        {
            _upgradeButton.image.color = _faded;
            _upgradeButton.interactable = false;
        }
    }

    public void CalculateNextUpgradeCost()
    {
        if (Upgrade.Equals(Upgrade.Upgrade2) && UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade2] >= 19)
        {
            _upgradeButton.GetComponentInChildren<Text>().text = "MAX";
            UpgradeLevel.text = "Level MAX";
        }
        else
        {
            int currentLevel = 0;
            var controller = UpgradesController.Instance;

            if (controller.UpgradesDictionary.ContainsKey(Upgrade))
            {
                currentLevel = controller.UpgradesDictionary[Upgrade];
            }
            else
            {
                currentLevel = 0;
                controller.UpgradesDictionary.Add(Upgrade, 0);
            }

            _nextUpgradeCost =
                UpgradesConsts.Exponent(_upgradeValues.BaseCost, _upgradeValues.CostMultiplier, currentLevel);

            _upgradeButton.GetComponentInChildren<Text>().text = "$" + MoneyConverter.ConvertNumber(_nextUpgradeCost);

            if (controller.UpgradesDictionary.ContainsKey(Upgrade))
            {
                UpgradeLevel.text = "Level " + controller.UpgradesDictionary[Upgrade];
            }
            else
            {
                controller.UpgradesDictionary.Add(Upgrade, 0);
            }

            UpdateSlider();
            GameController.Instance.SetMiningPowerText();
        }
    }

    public void UpdateSlider()
    {
        int level = UpgradesController.Instance.UpgradesDictionary[Upgrade];
        int maxLevel = 1;
        int previousMaxLevel = 0;

        if (UpgradesController.Instance.UpgradesMultipliers.ContainsKey(Upgrade))
        {

            foreach (var nextMultiply in UpgradesController.Instance.UpgradesMultipliers[Upgrade].Keys)
            {
                if (level < nextMultiply)
                {
                    maxLevel = nextMultiply;
                    break;
                }

                previousMaxLevel = nextMultiply;
            }
        }

        MultiplyProgressSlider.maxValue = maxLevel - previousMaxLevel;
        MultiplyProgressSlider.value = level - previousMaxLevel;
    }

    public void OnClick()
    {
        var controller = GameController.Instance;

        if (controller.GetMoney() >= _nextUpgradeCost)
        {
            if (!(Upgrade.Equals(Upgrade.Upgrade2) && UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade2] < 19))
            {
                UpgradesController.Instance.BuyUpgrade(Upgrade);
                controller.SubMoney(_nextUpgradeCost);
                CalculateNextUpgradeCost();

                if (Upgrade.Equals(Upgrade.Upgrade1))
                {
                    controller.CalculateStrength();
                }
                else if (Upgrade.Equals(Upgrade.Upgrade2))
                {
                    controller.CalculateMiningSpeed();
                }
                else
                {
                    controller.CalculateAutoStrength();
                }

                ToggleButtonFade();
            }
        }
    }
}
