using System;
using System.Collections;
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
    public int BaseAscendCost = 5;
    public int AscendBias = -1;
    public float AscendExponentialMultiplier = 1.12f;

    private double _money;
    private int _maxLevel = 1;
    private int _level = 1;
    private int _strength;
    private int _autoStrength;
    private double _nextLevelCost;
    private Vector3 _rockSpawn;
    private DateTime epochTimeStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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
        ChangeButtonColor();
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

    public void CalculateNextLevelCost()
    {
        _nextLevelCost = Mathf.Round(BaseAscendCost * Mathf.Pow(AscendExponentialMultiplier, _maxLevel) + AscendBias);
        AscendCostDisplay.text = "$" + MoneyConverter.ConvertNumber(_nextLevelCost);
        ChangeButtonColor();
    }

    public void CalculateStrength()
    {
        _strength = 1;
    }

    private void CalculateAutoStrength()
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
        Load();
        SetMoneyText();
        SetLevelText();
        CalculateNextLevelCost();
        CalculateAutoStrength();
        CalculateStrength();
        SetRockSpawn();
        SpawnRock();
    }

    private void ChangeButtonColor()
    {
        if (_nextLevelCost <= _money)
            AscendButton.SetActive();
        else
            AscendButton.SetInactive();
    }

    private void SetRockSpawn()
    {
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        _rockSpawn = new Vector3(0, (float) worldScreenHeight, 0);
    }

    public void Save()
    {
        var bf = new BinaryFormatter();
        var file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        PlayerData data = new PlayerData();
        data.MaxLevel = _maxLevel;
        data.Money = _money;
        data.Time = (int) (DateTime.UtcNow - epochTimeStart).TotalSeconds;

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

            PlayerData data = (PlayerData) bf.Deserialize(file);
            file.Close();

            _maxLevel = data.MaxLevel;
            _money = data.Money;

            int idleTime = (int) (DateTime.UtcNow - epochTimeStart).TotalSeconds - data.Time;

            if(idleTime > 0)
            {
                IdleEarnings.IdleReward(idleTime);
            }
        }
    }
}

[Serializable]
class PlayerData
{
    public int MaxLevel;
    public double Money;
    public int Time;
}