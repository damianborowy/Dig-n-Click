using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Text MoneyDisplay;
    public Text LevelDisplay;
    public Text AscendCostDisplay;
    public Text MiningPowerDisplay;
    public GameObject[] Rocks;
    public LevelChanger ArrowDownLevelChanger;
    public ProbabilitesHandler ProbabilitiesUI;
    public bool EnableSaving = true;

    public static readonly DateTime EpochTimeStart =
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private double _money;
    private int _maxLevel = 1;
    private int _level = 1;
    private double _strength = 1;
    private double _autoStrength = 0;
    private double _miningSpeed = 1.05;
    private double _nextLevelCost;
    private Vector3 _rockSpawn;
    private string _saveFilePath;
    private double _lastTime = (DateTime.UtcNow - EpochTimeStart).TotalSeconds;
    private bool _firstLaunch = true;
    private bool _tutorialCompleted = false;
    private GameObject[] _upgradesList;

    [Serializable]
    private class PlayerData
    {
        public int MaxLevel, Level;
        public double Time;
        public double Money;
        public bool FirstLaunch, TutorialCompleted;
        public Dictionary<string, int> Items;
        public Dictionary<Upgrade, int> Upgrades;
    }

    //Unity -----------------------------------------------------------------------------------

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
        _saveFilePath = Application.persistentDataPath + "/playerInfo.dat";
        _upgradesList = GameObject.FindGameObjectsWithTag("UpgradeSlot");

        if (EnableSaving)
        {
            Load();
        }

        SetMoneyText();
        SetLevelText();
        ArrowDownLevelChanger.UpdatePricetag();
        CalculateNextLevelCost();
        CalculateAutoStrength();
        CalculateStrength();
        SetRockSpawn();
        SpawnRock();
        CalculateMiningSpeed();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            _lastTime = (DateTime.UtcNow - EpochTimeStart).TotalSeconds;
    }

    //Launch ----------------------------------------------------------------------------------

    public bool IsFirstLaunch()
    {
        return _firstLaunch;
    }

    public void NotFirstLaunch()
    {
        _firstLaunch = false;
    }

    //Tutorial --------------------------------------------------------------------------------

    public bool IsTutorialCompleted()
    {
        return _tutorialCompleted;
    }

    public void CompleteTutorial()
    {
        _tutorialCompleted = true;
    }


    //Money -----------------------------------------------------------------------------------

    public double GetMoney()
    {
        return _money;
    }

    public void AddMoney(double added)
    {
        _money += added;
        SetMoneyText();
        ToggleUpgradeButtons();
    }

    public void SubMoney(double subbed)
    {
        _money -= subbed;
        SetMoneyText();
        ToggleUpgradeButtons();
    }

    private void SetMoneyText()
    {
        MoneyDisplay.text = "$" + MoneyConverter.ConvertNumber(_money);
    }

    //Upgrades --------------------------------------------------------------------------------

    public void ToggleUpgradeButtons()
    {
        foreach (var slot in _upgradesList)
        {
            slot.GetComponentInChildren<UpgradePanelHandler>().ToggleButtonFade();
        }
    }

    public void ToggleUpgradeSlots()
    {
        foreach (var slot in _upgradesList)
        {
            slot.GetComponentInChildren<UpgradePanelHandler>().ToggleSlot();
        }
    }

    //IdleTime --------------------------------------------------------------------------------

    public double GetIdleTime()
    {
        return (DateTime.UtcNow - EpochTimeStart).TotalSeconds - _lastTime;
    }

    //Rock ------------------------------------------------------------------------------------

    private void SetRockSpawn()
    {
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        _rockSpawn = new Vector3(0, (float) worldScreenHeight, 0);
    }

    public void SpawnRock()
    {
        Instantiate(Rocks[UnityEngine.Random.Range(0, Rocks.Length)], _rockSpawn, Quaternion.identity);
        GameObject shadow = GameObject.FindWithTag("Shadow");
        if (shadow != null)
            shadow.GetComponent<ShadowResizer>().Resize();
        else
        {
            Debug.Log("Shadow not found");
        }

        AutoMiner.Instance.StartMiner();
    }

    //MiningPower -----------------------------------------------------------------------------

    public double GetMiningSpeed()
    {
        return _miningSpeed;
    }

    public void SetMiningSpeed(double speed)
    {
        _miningSpeed = speed;
    }

    public void CalculateMiningSpeed()
    {
        AutoMiner.Instance.StopMiner();

        if (UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade.Upgrade2))
        {
            SetMiningSpeed(1.05 - UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade2] *
                           UpgradesConsts.GetUpgradeValues(Upgrade.Upgrade2).Productivity);
        }
        else
        {
            _miningSpeed = 1.05;
        }

        AutoMiner.Instance.StartMiner();
    }

    public double GetStrength()
    {
        return _strength;
    }

    public void CalculateStrength()
    {
        if (UpgradesController.Instance.UpgradesDictionary.ContainsKey(Upgrade.Upgrade1))
        {
            _strength = (UpgradesController.Instance.UpgradesDictionary[Upgrade.Upgrade1] + 1) *
                        UpgradesController.CalculateMultiplier(Upgrade.Upgrade1);
        }
        else
        {
            _strength = 1;
            UpgradesController.Instance.UpgradesDictionary.Add(Upgrade.Upgrade1, 0);
        }
    }

    public double GetAutoStrength()
    {
        return _autoStrength;
    }

    public void CalculateAutoStrength()
    {
        double autoStrength = 0;
        UpgradesConsts.UpgradeValues values;

        if (UpgradesController.Instance.UpgradesDictionary.Count != 0)
        {
            foreach (var pair in UpgradesController.Instance.UpgradesDictionary)
            {
                if (!pair.Key.Equals(Upgrade.Upgrade1) && !pair.Key.Equals(Upgrade.Upgrade2))
                {
                    values = UpgradesConsts.GetUpgradeValues(pair.Key);
                    autoStrength += values.Productivity * UpgradesController.Instance.UpgradesDictionary[pair.Key] *
                                    UpgradesController.CalculateMultiplier(pair.Key);
                }
            }
        }

        _autoStrength = autoStrength;
    }

    public void SetMiningPowerText()
    {
        MiningPowerDisplay.text =
            MoneyConverter.ConvertNumber(Math.Round(GetAutoStrength() / GetMiningSpeed())) + " DPS";
    }

    //Level -----------------------------------------------------------------------------------

    public int GetLevel()
    {
        return _level;
    }

    public void SetLevel(int level)
    {
        _level = level;
        SetLevelText();
        ProbabilitiesUI.UpdateProbabilities();
        ArrowDownLevelChanger.UpdatePricetag();
    }

    public int GetMaxLevel()
    {
        return _maxLevel;
    }

    public void AddMaxLevel()
    {
        SubMoney(_nextLevelCost);
        ++_maxLevel;
        ArrowDownLevelChanger.UpdatePricetag();
        SetLevelText();
        CalculateNextLevelCost();
    }

    public double GetNextLevelCost()
    {
        return _nextLevelCost;
    }

    public void CalculateNextLevelCost()
    {
        _nextLevelCost = BasicEconomyValues.Exponent(BasicEconomyValues.BaseAscendCost,
            BasicEconomyValues.AscendBias,
            BasicEconomyValues.AscendExponentialMultiplier,
            _maxLevel);
        AscendCostDisplay.text = "$" + MoneyConverter.ConvertNumber(_nextLevelCost);
    }

    private void SetLevelText()
    {
        LevelDisplay.text = _level.ToString();
    }

    //Save & Load -----------------------------------------------------------------------------

    public void Save()
    {
        var bf = new BinaryFormatter();
        var file = File.Open(_saveFilePath, FileMode.Open);

        PlayerData data = new PlayerData();
        data.MaxLevel = _maxLevel;
        data.Level = _level;
        data.Money = _money;
        data.Time = (DateTime.UtcNow - EpochTimeStart).TotalSeconds;
        data.Items = SerializableOre.ConvertToSerializable(EquipmentController.Instance.Items);
        data.Upgrades = UpgradesController.Instance.UpgradesDictionary;
        data.TutorialCompleted = _tutorialCompleted;
        data.FirstLaunch = false;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(_saveFilePath) && new FileInfo(_saveFilePath).Length != 0)
        {
            var bf = new BinaryFormatter();
            var file = File.Open(_saveFilePath, FileMode.Open);

            PlayerData data = (PlayerData) bf.Deserialize(file);
            file.Close();

            _maxLevel = data.MaxLevel;
            _level = data.Level;
            _money = data.Money;
            _lastTime = data.Time;
            _firstLaunch = data.FirstLaunch;
            _tutorialCompleted = data.TutorialCompleted;
            UpgradesController.Instance.UpgradesDictionary = data.Upgrades;

            SerializableOre.DeserializeOres(data.Items);
        }
        else if (!File.Exists(_saveFilePath))
        {
            File.Create(_saveFilePath).Dispose();
        }
    }
}