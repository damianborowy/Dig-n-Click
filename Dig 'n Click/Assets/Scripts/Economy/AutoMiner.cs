using System;
using System.Collections;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    public static AutoMiner Instance;

    private RockController _rc;
    private int _miningSpeedMultiplier = 1;

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
        StartCoroutine(AutoMine());
    }

    private IEnumerator AutoMine()
    {
        while (true)
        {
            _rc = GetRockController();

            while (_rc != null)
            {
                _rc.Hit(CalculateHitStrength());
                yield return new WaitForSeconds((float) GameController.Instance.GetMiningSpeed() / _miningSpeedMultiplier);
            }

            yield return null;
        }
    }

    private RockController GetRockController()
    {
        GameObject rock = GameObject.FindWithTag("Rock");
        return rock != null ? rock.GetComponent<RockController>() : null;
    }

    private double CalculateHitStrength()
    {
        return GameController.Instance.GetAutoStrength();
    }

    public int GetMiningSpeedMultiplier()
    {
        return _miningSpeedMultiplier;
    }

    public IEnumerator MultiplyDamagePerSecond(int times, float seconds)
    {
        _miningSpeedMultiplier *= times;
        GameController.Instance.UpdateMiningPowerText();
        yield return new WaitForSecondsRealtime(seconds);
        _miningSpeedMultiplier /= times;
        GameController.Instance.UpdateMiningPowerText();
    }
}