using System;
using System.Collections;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    public static AutoMiner Instance;

    private RockController _rc;

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
                _rc.Hit(GameController.Instance.GetAutoStrength());
                yield return new WaitForSeconds((float) GameController.Instance.GetMiningSpeed());
            }

            yield return null;
        }
    }

    private RockController GetRockController()
    {
        GameObject rock = GameObject.FindWithTag("Rock");
        return rock != null ? rock.GetComponent<RockController>() : null;
    }
}