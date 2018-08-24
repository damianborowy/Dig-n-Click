using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class AscendLevelOnClick : MonoBehaviour
{
    public void OnClick()
    {
        if (GameController.Instance.GetNextLevelCost() <= GameController.Instance.GetMoney())
            GameController.Instance.AddMaxLevel();
    }
}