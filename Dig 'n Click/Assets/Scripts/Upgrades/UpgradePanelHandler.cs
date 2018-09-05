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
    public Text ActualProductivity;
    public Text NextMultiplier;
    public Slider MultiplyProgressSlider;
    public AudioClip BuySound;

    private UpgradesConsts.UpgradeValues _upgradeValues;
    private double _nextUpgradeCost;
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
        UpdateSlider();
        GameController.Instance.SetMiningPowerText();
        UpdateText();
        ToggleSlot();
    }

    private void UpdateText()
    {
        if (Upgrade.Equals(Upgrade.Upgrade1))
        {
            UpgradeText.text = "Increases DMG by " + _upgradeValues.Productivity;
            ActualProductivity.text = "Current DMG is " + GameController.Instance.GetStrength();

            if (!UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade)) return;

            NextMultiplier.text =
                "x" + UpgradesController.GetClosestMultiplier(Upgrade);
        }
        else if (Upgrade.Equals(Upgrade.Upgrade2))
        {
            UpgradeText.text = "Shortens auto mining interval by " + _upgradeValues.Productivity;
            ActualProductivity.text = "Current mining interval is " + GameController.Instance.GetMiningSpeed() + "s";
            NextMultiplier.text = "";
        }
        else
        {
            UpgradeText.text = "Increases DPS by " + _upgradeValues.Productivity;
            ActualProductivity.text = "Current DPS is " + UpgradesController.CalculateUpgradeProductivity(Upgrade);

            if (!UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade)) return;

            NextMultiplier.text =
                "x" + UpgradesController.GetClosestMultiplier(Upgrade);
        }
    }

    public void ToggleButtonFade()
    {
        if (GameController.Instance.GetMoney() >= _nextUpgradeCost)
        {
            _upgradeButton.image.color = _upgradeButton.colors.normalColor;
            _upgradeButton.interactable = true;
        }
        else
        {
            _upgradeButton.image.color = _upgradeButton.colors.disabledColor;
            _upgradeButton.interactable = false;
        }

        if (UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade.Upgrade2))
        {
            if (!Upgrade.Equals(Upgrade.Upgrade2) ||
                UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade2] < 20) return;

            _upgradeButton.image.color = _upgradeButton.colors.disabledColor;
            _upgradeButton.interactable = false;
        }
    }

    public void CalculateNextUpgradeCost()
    {
        var controller = UpgradesController.Instance;

        if (Upgrade.Equals(Upgrade.Upgrade2) &&
            UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade2] >= 20)
        {
            _upgradeButton.GetComponentInChildren<Text>().text = "MAX";
            UpgradeLevel.text = "Level MAX";
            return;
        }

        int currentLevel = controller.UpgradesDictionary[Upgrade];

        _nextUpgradeCost =
            UpgradesConsts.Exponent(_upgradeValues.BaseCost, _upgradeValues.CostMultiplier, currentLevel);

        _upgradeButton.GetComponentInChildren<Text>().text = "$" + MoneyConverter.ConvertNumber(_nextUpgradeCost);

        UpgradeLevel.text = "Level " + controller.UpgradesDictionary[Upgrade];

        ToggleButtonFade();
        UpdateSlider();
        GameController.Instance.SetMiningPowerText();
        UpdateText();
    }

    public void UpdateSlider()
    {
        if (!UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade)) return;

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

    public void ToggleSlot()
    {
        if (Upgrade.Equals(Upgrade.Upgrade1) || Upgrade.Equals(Upgrade.Upgrade2) || Upgrade.Equals(Upgrade.Upgrade3) ||
            !UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade.Previous())) return;

        gameObject.SetActive(UpgradesController.Instance.UpgradesDictionary[Upgrade.Previous()] != 0);
    }

    public void OnClick()
    {
        AudioController.Instance.PlayBuySellSound(BuySound);

        var controller = GameController.Instance;

        if (Upgrade.Equals(Upgrade.Upgrade2) &&
            UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade2] >= 20) return;

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
        UpdateSlider();
        controller.SetMiningPowerText();
        UpdateText();
    }
}