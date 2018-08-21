using System.Collections;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    private RockController _rc;

    private void Start()
    {
        _rc = GetRockController();
        StartCoroutine(AutoMine());
    }

    private IEnumerator AutoMine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if (_rc == null)
            {
                _rc = GetRockController();
            }
            else
            {
                _rc.Hit(GameController.Instance.GetAutoStrength());
            }
        }
    }

    private RockController GetRockController()
    {
        GameObject rock = GameObject.FindWithTag("Rock");
        return rock != null ? rock.GetComponent<RockController>() : null;
    }
}