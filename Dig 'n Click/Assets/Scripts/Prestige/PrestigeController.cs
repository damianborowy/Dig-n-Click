using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrestigeController : MonoBehaviour
{
    public static PrestigeController Instance;
    public GameObject UpgradesPanel;
    public Text OwnedAmount;
    public Text RewardedAmount;
    public Text MultiplierText;
    public Text LevelText;
    public Button PrestigeButton;

    public float TimeBetweenLevelChange;
    public AudioClip CrystalSound;
    public BackgroundScroller Scroller;
    public GameObject ShadowGameObject;
    public GameObject SliderGameObject;
    public GameObject OreProbabilitesGameObject;
    public GameObject PriceTagGameObject;
    public GameObject AnimatedPrestige;

    private int _prestigeLevel;
    private bool _isLevelChangeFinished;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateOwnedAmount();
    }

    private double CalculateReward()
    {
        var itemsList = EquipmentController.Instance.Items;
        double totalMoney = GameController.Instance.GetTotalMoney();

        foreach (KeyValuePair<Ore, int> pair in itemsList)
        {
            totalMoney += pair.Key.Value * pair.Value;
        }

        double prestigeReward = 150 * Math.Sqrt(totalMoney / Math.Pow(10, 12));

        Debug.Log("Current prestige reward: " + prestigeReward);

        return Math.Ceiling(prestigeReward);
    }

    private void UpdateOwnedAmount()
    {
        OwnedAmount.text = MoneyConverter.ConvertNumber(GameController.Instance.GetPrestigeCrystals());
    }

    public void UpdateReward()
    {
        var controller = GameController.Instance;
        var reward = CalculateReward();

        RewardedAmount.text = "+" + MoneyConverter.ConvertNumber(reward);
        PrestigeButton.interactable = !(reward < 1);
        MultiplierText.text = "Your current global multiplier x" +
                              Math.Round(1 + 1 * controller.GetPrestigeCrystals() * controller.GetPrestigeCrystalsMultiplier(),
                                  2) +
                              " will increase to x" +
                              Math.Round(1 + 1 * reward * controller.GetPrestigeCrystalsMultiplier() +
                                         1 * controller.GetPrestigeCrystals() * controller.GetPrestigeCrystalsMultiplier(), 2);
    }

    public void OnClick()
    {
        DoPrestige(CalculateReward());
    }

    public void DoPrestige(double prestigeCrystalsToBeAdded)
    {
        UpgradesController.Instance.OverrideUpgrades();
        foreach (Transform childUpgradeTransform in UpgradesPanel.transform)
        {
            var upgradePanelHandler = childUpgradeTransform.gameObject.GetComponent<UpgradePanelHandler>();
            upgradePanelHandler.CalculateNextUpgradeCost();
            upgradePanelHandler.ToggleSlot();
        }

        EquipmentController.Instance.Items = new SortedList<Ore, int>(new OreCompareByValue());
        EquipmentController.Instance.UpdateItemSlots();

        var controller = GameController.Instance;
        controller.AddPrestigeCrystals(prestigeCrystalsToBeAdded);
        _prestigeLevel = controller.GetLevel();
        controller.SetMaxLevel(1);
        controller.SetLevel(1);
        controller.SetMoneyToZero();
        controller.CalculateAutoStrength();
        controller.CalculateMiningSpeed();
        controller.CalculateStrength();

        UpdateOwnedAmount();
        UpdateReward();

        StartCoroutine(StartAnimation());
    }

    public IEnumerator StartAnimation()
    {
        GameObject instantiatedAnimatedPrestigeGameObject = Instantiate(AnimatedPrestige);
        ParticleSystem particleSystem = instantiatedAnimatedPrestigeGameObject.GetComponent<ParticleSystem>();

        OreProbabilitesGameObject.SetActive(false);
        ShadowGameObject.SetActive(false);
        SliderGameObject.SetActive(false);
        PriceTagGameObject.SetActive(false);
        GameObject rock = GameObject.FindWithTag("Rock");
        if (rock != null)
            Destroy(rock);

        AudioController.Instance.PlayCrystalSound(CrystalSound);

        StartCoroutine(ChangeLevel());
        while (!_isLevelChangeFinished)
        {
            if (!Scroller.IsBackgroundScrolling())
                Scroller.ScrollBackground(Direction.Up);

            yield return null;
        }

        particleSystem.Stop();
        Destroy(instantiatedAnimatedPrestigeGameObject, particleSystem.main.duration);

        AudioController.Instance.StopCrystalSound();

        OreProbabilitesGameObject.SetActive(true);
        ShadowGameObject.SetActive(true);
        SliderGameObject.SetActive(true);
        PriceTagGameObject.SetActive(true);
        GameController.Instance.SpawnRock();
    }

    private IEnumerator ChangeLevel()
    {
        while (_prestigeLevel != 1)
        {
            LevelText.text = (--_prestigeLevel).ToString();
            yield return new WaitForSeconds(TimeBetweenLevelChange);
        }

        _isLevelChangeFinished = true;
    }
}