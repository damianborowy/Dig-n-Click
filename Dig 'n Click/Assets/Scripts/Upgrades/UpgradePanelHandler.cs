using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelHandler : MonoBehaviour
{
    public Upgrade Upgrade;
    public TextMeshProUGUI UpgradeDescription;
    public Text UpgradeLevel;
    public TextMeshProUGUI ActualProductivity;
    public Text NextMultiplier;
    public Slider MultiplyProgressSlider;
    public AudioClip BuySound;
    public float BuySoundVolume;

    private UpgradesConsts.UpgradeValues _upgradeValues;
    private double _nextUpgradeCost;
    private Button _upgradeButton;

    private void Awake()
    {
        _upgradeButton = gameObject.GetComponentInChildren<Button>();
        _upgradeValues = UpgradesConsts.GetUpgradeValues(Upgrade);
    }

    private void Start()
    {
        CalculateNextUpgradeCost();
        ToggleSlot();
    }

    private void UpdateText()
    {
        var prestigeBonus = GameController.Instance.GetPrestigeCrystals() *
                            GameController.Instance.GetPrestigeCrystalsMultiplier();

        var productivity = _upgradeValues.Productivity * UpgradesController.CalculateMultiplier(Upgrade);
        productivity += productivity * prestigeBonus;

        if (Upgrade.Equals(Upgrade.Upgrade1))
        {
            var strength = GameController.Instance.GetStrength();

            UpgradeDescription.text =
                "Increases <b>DMG</b> by <b>" + MoneyConverter.ConvertNumber(productivity) + "</b>.";
            ActualProductivity.text = "Current <b>DMG</b> is <b>" + MoneyConverter.ConvertNumber(strength) + "</b>.";

            if (!UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade)) return;

            NextMultiplier.text =
                "x" + UpgradesController.GetClosestMultiplier(Upgrade);
        }
        else if (Upgrade.Equals(Upgrade.Upgrade2))
        {
            UpgradeDescription.text = "Shortens auto mining interval by <b>" + _upgradeValues.Productivity + "s</b>.";
            ActualProductivity.text =
                "Current mining interval is <b>" + GameController.Instance.GetMiningSpeed() + "s</b>.";
            NextMultiplier.text = "";
        }
        else
        {
            var autoStrength = UpgradesController.CalculateUpgradeProductivity(Upgrade);
            autoStrength += autoStrength * prestigeBonus;

            UpgradeDescription.text =
                "Increases <b>DPS</b> by <b>" + MoneyConverter.ConvertNumber(productivity) + "</b>.";
            ActualProductivity.text =
                "Current <b>DPS</b> is <b>" + MoneyConverter.ConvertNumber(autoStrength) + "</b>.";

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
            UpdateText();
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
        GameController.Instance.UpdateMiningPowerText();
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
        AudioController.Instance.PlayAudioEffect(BuySound, BuySoundVolume);

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
        UpdateText();
    }
}