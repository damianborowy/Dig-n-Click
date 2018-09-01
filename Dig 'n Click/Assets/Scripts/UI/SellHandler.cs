using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellHandler : MonoBehaviour
{
    public AudioClip SellSound;

    private Ore _oreToSell;
    private int _amountSet;

    public void Initialize(Ore ore, int maxAmount)
    {
        _oreToSell = ore;
        transform.GetChild(0).Find("Name").gameObject.GetComponent<Text>().text = ore.Name;
        transform.GetChild(0).Find("Slider").gameObject.GetComponent<Slider>().maxValue = maxAmount;
    }

    public void OnValueChange(float amount)
    {
        _amountSet = (int) amount;
        transform.GetChild(0).Find("Amount").gameObject.GetComponent<Text>().text = amount.ToString();
        transform.GetChild(0).Find("SellButton").GetChild(0).gameObject.GetComponent<Text>().text =
            "+$" + MoneyConverter.ConvertNumber(amount * _oreToSell.Value);
    }

    public void OnClick()
    {
        if (EquipmentController.Instance.RemoveItem(_oreToSell, _amountSet))
        {
            AudioController.Instance.PlayBuySellSound(SellSound);
            GameController.Instance.AddMoney(_oreToSell.Value * _amountSet);
        }
    }
}