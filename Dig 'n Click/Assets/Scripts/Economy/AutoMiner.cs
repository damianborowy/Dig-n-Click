using System.Collections;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    private RockController _rc;

    private void Start()
    {
        _rc = GameObject.FindGameObjectWithTag("Rock").GetComponent<RockController>();
        StartCoroutine(AutoMine());
    }

    private IEnumerator AutoMine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            if(_rc == null)
            {
                _rc = GameObject.FindGameObjectWithTag("Rock").GetComponent<RockController>();
            }
            else
            {
                _rc.Hit(1);
            }
        }
    }
}