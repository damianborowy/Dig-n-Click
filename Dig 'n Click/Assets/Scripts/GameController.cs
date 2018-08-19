using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Text MoneyDisplay;
    public Text LevelDisplay;
    public GameObject[] Rocks;

    private double _money;
    private int _level = 1;
    private Vector3 _rockSpawn;

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

    public void SpawnRock()
    {
        Instantiate(Rocks[Random.Range(0, Rocks.Length)], _rockSpawn, Quaternion.identity);
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
        SetRockSpawn();
        SpawnRock();
    }

    private void Update()
    {
        MoneyDisplay.text = "Money: " + _money;
        LevelDisplay.text = "Level: " + _level;
    }

    private void SetRockSpawn()
    {
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        _rockSpawn = new Vector3(0, (float) worldScreenHeight, 0);
    }
}