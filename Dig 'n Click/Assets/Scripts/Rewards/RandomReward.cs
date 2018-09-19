using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomReward : MonoBehaviour
{
    public GameObject RewardText;
    public AudioClip CandyPopSound;
    public float CandyPopSoundVolume;

    private const double MoneyRewardMinutesMining = 5;
    private const double OreRewardMinutesMining = 5;
    private const int PrestigeCrystalsRewardAmount = 1;
    private const int DPSRewardMultiplier = 2;
    private const float DPSRewardDurationInSeconds = 15;

    private Reward[] _rewards;

    private abstract class Reward
    {
        private GameObject _rewardText;
        private Transform _targetTransform;

        protected Reward(GameObject rewardText, Transform targetTransform)
        {
            _rewardText = rewardText;
            _targetTransform = targetTransform;
        }

        public abstract void GetReward();

        protected static int GetCurrentRocksDestroyed(double time)
        {
            int currentLevel = GameController.Instance.GetLevel();
            double damagePerSecond = GameController.Instance.GetMiningPower();
            return RockValuesCalculator.GetRocksDestroyedByTime(currentLevel, damagePerSecond, time);
        }

        protected void CreateRewardText(string text, Color color)
        {
            Text rewardText = CreateTextGameobject();
            rewardText.text = text;
            rewardText.color = color;
            rewardText.rectTransform.position = _targetTransform.position;
        }

        private Text CreateTextGameobject()
        {
            RectTransform fadingTextSpawn = GameObject.FindWithTag("FadingTextSpawn").GetComponent<RectTransform>();
            GameObject instantiatedTextGameObject = Instantiate(_rewardText, fadingTextSpawn);
            Text text = instantiatedTextGameObject.GetComponent<Text>();
            return text;
        }
    }

    private class MoneyReward : Reward
    {
        public MoneyReward(GameObject rewardText, Transform targetTransform) : base(rewardText, targetTransform)
        {
        }

        public override void GetReward()
        {
            double moneyRewardSecondsMining = MoneyRewardMinutesMining * 60;
            int rocksDestroyed = GetCurrentRocksDestroyed(moneyRewardSecondsMining);
            double moneyToBeAdded = GetMoneyToBeAdded(rocksDestroyed);
            GameController.Instance.AddMoney(moneyToBeAdded);
            CreateMoneyRewardText(moneyToBeAdded);
        }

        private static double GetMoneyToBeAdded(int rocksDestroyed)
        {
            int level = GameController.Instance.GetLevel();
            return BasicEconomyValues.MoneyReward(level) * rocksDestroyed;
        }

        private void CreateMoneyRewardText(double money)
        {
            string moneyRewardString = "+$" + MoneyConverter.ConvertNumber(money);
            CreateRewardText(moneyRewardString, Color.yellow);
        }
    }

    private class OreReward : Reward
    {
        public OreReward(GameObject rewardText, Transform targetTransform) : base(rewardText, targetTransform)
        {
        }

        public override void GetReward()
        {
            double oreRewardSecondsMining = OreRewardMinutesMining * 60;
            int rocksDestroyed = GetCurrentRocksDestroyed(oreRewardSecondsMining);
            OreDropper dropper = GameObject.FindWithTag("Dropper").GetComponent<OreDropper>();
            dropper.DropOre(rocksDestroyed);
            CreateOreRewardText();
        }

        private void CreateOreRewardText()
        {
            string oreRewardString = "Random ores!";
            CreateRewardText(oreRewardString, Color.white);
        }
    }

    private class PrestigeCrystalReward : Reward
    {
        public PrestigeCrystalReward(GameObject rewardText, Transform targetTransform) : base(rewardText,
            targetTransform)
        {
        }

        public override void GetReward()
        {
            GameController.Instance.AddPrestigeCrystals(PrestigeCrystalsRewardAmount);
            PrestigeController.Instance.UpdateOwnedAmount();
            CreatePrestigeCrystalRewardText();
        }

        private void CreatePrestigeCrystalRewardText()
        {
            string oreRewardString = "+" + PrestigeCrystalsRewardAmount + " Prestige Crystal";
            Color purple = new Color(0.561f, 0.137f, 1);
            CreateRewardText(oreRewardString, purple);
        }
    }

    private class DPSReward : Reward
    {
        public DPSReward(GameObject rewardText, Transform targetTransform) : base(rewardText, targetTransform)
        {
        }

        public override void GetReward()
        {
            AutoMiner.Instance.StartCoroutine(
                AutoMiner.Instance.MultiplyDamagePerSecond(DPSRewardMultiplier, DPSRewardDurationInSeconds));
            CreateDPSRewardText();
        }

        private void CreateDPSRewardText()
        {
            string oreRewardString = "Mining frenzy!";
            CreateRewardText(oreRewardString, Color.yellow);
        }
    }

    private void Awake()
    {
        _rewards = new Reward[]
        {
            new MoneyReward(RewardText, transform),
            new OreReward(RewardText, transform),
            new PrestigeCrystalReward(RewardText, transform),
            new DPSReward(RewardText, transform) 
        };
    }

    public void GetReward()
    {
        AudioController.Instance.PlayAudioEffect(CandyPopSound, CandyPopSoundVolume);
        PickRandomReward().GetReward();
        //TODO Add DPS bonus reward.
        Destroy(gameObject);
    }

    private Reward PickRandomReward()
    {
        return _rewards[Random.Range(0, _rewards.Length)];
    }
}