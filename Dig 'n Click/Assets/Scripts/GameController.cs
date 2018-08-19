using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Text MoneyDisplay;
    public Text LevelDisplay;

    private double _money;
    private int _level = 1;

    public void AddMoney(double added)
    {
        _money += added;
    }
    public void SubMoney(double subbed)
    {
        _money -= subbed;
    }
    public double GetMoney()
    {
        return _money;
    }
    public int GetLevel()
    {
        return _level;
    }
    public void SetLevel(int level)
    {
        this._level = level;
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

    private void Update()
    {
        MoneyDisplay.text = "Money: " + _money;
        LevelDisplay.text = "Level: " + _level;
    }
}