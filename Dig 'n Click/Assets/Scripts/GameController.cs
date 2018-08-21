using System.Collections;
using System.Collections.Generic;
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

    private double _money;
    private int _level = 1;
    private int _strength;
    private int _autoStrength;
    private double _nextLevelCost;
    private Vector3 _rockSpawn;

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

    public int GetLevel()
    {
        return _level;
    }

    public void AddLevel()
    {
        SubMoney(_nextLevelCost);
        this._level++;
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
        _nextLevelCost = Mathf.Round(5 * Mathf.Pow(1.12f, _level) + -1);
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
        Instantiate(Rocks[Random.Range(0, Rocks.Length)], _rockSpawn, Quaternion.identity);
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
}