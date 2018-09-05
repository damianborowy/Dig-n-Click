using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeController : MonoBehaviour
{
    public static PrestigeController Instance;
    public Text OwnedAmount;
    public Text RewardedAmount;
    public Text MultiplierText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            UpdateOwnedAmount();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private double CalculateReward()
    {
        var itemsList = EquipmentController.Instance.Items;
        double totalMoney = GameController.Instance.GetMoneySincePrestige();

        foreach (KeyValuePair<Ore, int> pair in itemsList)
        {
            totalMoney += pair.Key.Value * pair.Value;
        }

        double prestigeReward = Math.Pow(totalMoney / Math.Pow(10, 4), 0.5);

        return prestigeReward;
    }

    private void UpdateOwnedAmount()
    {
        OwnedAmount.text = MoneyConverter.ConvertNumber(GameController.Instance.GetPrestigeOre());
    }

    public void UpdateRewardText()
    {
        var controller = GameController.Instance;
        var reward = CalculateReward();

        RewardedAmount.text = MoneyConverter.ConvertNumber(reward);
        MultiplierText.text = "Your current global multiplier x" +
                              (1 + 1 * controller.GetPrestigeOre() * controller.GetPrestigeOreMultiplier()) +
                              " will increase to x" +
                              Math.Round(1 + 1 * reward * controller.GetPrestigeOreMultiplier() + 
                              1 * controller.GetPrestigeOre() * controller.GetPrestigeOreMultiplier(), 2);

    }

    public void OnClick()
    {
        DoPrestige(CalculateReward());
    }

    public void DoPrestige(double prestigeOreToBeAdded)
    {
        var controller = GameController.Instance;
        var upgradesList = GameObject.FindGameObjectsWithTag("UpgradeSlot");

        UpgradesController.Instance.OverrideUpgrades();
        EquipmentController.Instance.Items = new SortedList<Ore, int>(new OreCompareByValue());
        EquipmentController.Instance.UpdateItemSlots();
        controller.AddPrestigeOre(prestigeOreToBeAdded);
        controller.SetLevel(1);
        controller.SetMaxLevel(1);
        controller.SetMoneyToZero();
        controller.CalculateAutoStrength();
        controller.CalculateMiningSpeed();
        controller.CalculateStrength();

        foreach (var upgrade in upgradesList)
        {
            upgrade.GetComponentInChildren<UpgradePanelHandler>().CalculateNextUpgradeCost();
        }

        UpdateOwnedAmount();
        UpdateRewardText();

        // here's some place for code doing some destructions or sth
    }
}