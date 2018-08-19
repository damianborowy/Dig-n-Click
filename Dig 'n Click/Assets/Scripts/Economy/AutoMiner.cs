using System.Collections;
using UnityEngine;

public class AutoMiner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(AutoMine());
    }
    //comment
    private IEnumerator AutoMine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            GameController.Instance.AddMoney(1);
        }
    }
}