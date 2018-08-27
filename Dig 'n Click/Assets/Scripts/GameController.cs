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
    public GameObject[] Rocks;
    public ButtonActivator AscendButton;
    public bool EnableSaving = true;
    public static readonly DateTime EpochTimeStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private double _money;
    private int _maxLevel = 1;
    private int _level = 1;
    private int _strength = 1;
    private int _autoStrength = 1;
    private double _nextLevelCost;
    private Vector3 _rockSpawn;
    private string _saveFilePath;
    private int _idleTime = (int) (DateTime.UtcNow - EpochTimeStart).TotalSeconds;
    private bool _firstLaunch = true;
    private bool _tutorialCompleted = false;

    public bool IsFirstLaunch()
    {
        return _firstLaunch;
    }

    public void NotFirstLaunch()
    {
        _firstLaunch = false;
    }

    public bool IsTutorialCompleted()
    {
        return _tutorialCompleted;
    }

    public void CompleteTutorial()
    {
        _tutorialCompleted = true;
    }

    public int GetStrength()
    {
        return _strength;
    }

    public int GetAutoStrength()
    {
        return _autoStrength;
    }

    public void AddMoney(double added)
    {
        _money += added;
        SetMoneyText();
    }

    public void SubMoney(double subbed)
    {
        _money -= subbed;
        SetMoneyText();
    }

    private void SetMoneyText()
    {
        MoneyDisplay.text = "$" + MoneyConverter.ConvertNumber(_money);
    }

    public double GetMoney()
    {
        return _money;
    }

    public int GetMaxLevel()
    {
        return _maxLevel;
    }

    public int GetLevel()
    {
        return _level;
    }

    public void SetLevel(int level)
    {
        _level = level;
        SetLevelText();
        ToggleButtonVisibility();
    }

    public void ToggleButtonVisibility()
    {
        if (_level != _maxLevel)
        {
            AscendButton.Hide();
        }
        else
        {
            AscendButton.Show();
        }
    }

    public void AddMaxLevel()
    {
        SubMoney(_nextLevelCost);
        this._maxLevel++;
        ToggleButtonVisibility();
        SetLevelText();
        CalculateNextLevelCost();
    }

    private void SetLevelText()
    {
        LevelDisplay.text = "Level: " + _level;
    }

    public double GetNextLevelCost()
    {
        return _nextLevelCost;
    }

    public int GetIdleTime()
    {
        return _idleTime;
    }

    public void CalculateNextLevelCost()
    {
        _nextLevelCost = BasicEconomyValues.Exponent(BasicEconomyValues.BaseAscendCost,
            BasicEconomyValues.AscendBias,
            BasicEconomyValues.AscendExponentialMultiplier,
            _maxLevel);
        AscendCostDisplay.text = "$" + MoneyConverter.ConvertNumber(_nextLevelCost);
    }

    public void CalculateStrength()
    {
        _strength = 1;
    }

    public void CalculateAutoStrength()
    {
        _autoStrength = 1;
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
    }

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

        if (EnableSaving)
        {
            Load();
        }

        SetMoneyText();
        SetLevelText();
        ToggleButtonVisibility();
        CalculateNextLevelCost();
        CalculateAutoStrength();
        CalculateStrength();
        SetRockSpawn();
        SpawnRock();
    }

    private void SetRockSpawn()
    {
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        _rockSpawn = new Vector3(0, (float) worldScreenHeight, 0);
    }

    public void Save()
    {
        var bf = new BinaryFormatter();
        var file = File.Open(_saveFilePath, FileMode.Open);

        PlayerData data = new PlayerData();
        data.MaxLevel = _maxLevel;
        data.Level = _level;
        data.Money = _money;
        data.Time = (int) (DateTime.UtcNow - EpochTimeStart).TotalSeconds;
        data.Items = SerializableOre.ConvertToSerializable(EquipmentController.Instance.Items);
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
            _idleTime = data.Time;
            _firstLaunch = data.FirstLaunch;
            _tutorialCompleted = data.TutorialCompleted;

            SerializableOre.DeserializeOres(data.Items);
        }
        else if (!File.Exists(_saveFilePath))
        {
            File.Create(_saveFilePath).Dispose();
        }
    }

    [Serializable]
    private class PlayerData
    {
        public int MaxLevel, Level, Time;
        public double Money;
        public bool FirstLaunch, TutorialCompleted;
        public Dictionary<string, int> Items;
    }
}