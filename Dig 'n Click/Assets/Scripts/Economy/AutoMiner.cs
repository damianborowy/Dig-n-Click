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

    public void StartMiner()
    {
        StartCoroutine(AutoMine());
    }

    public void StopMiner()
    {
        StopAllCoroutines();
    }

    private IEnumerator AutoMine()
    {
        yield return new WaitForSeconds(0.45f);
        _rc = GetRockController();

        while (true)
        {
            _rc.Hit(GameController.Instance.GetAutoStrength());
            yield return new WaitForSeconds((float) GameController.Instance.GetMiningSpeed());
        }
    }

    private RockController GetRockController()
    {
        GameObject rock = GameObject.FindWithTag("Rock");
        return rock != null ? rock.GetComponent<RockController>() : null;
    }
}