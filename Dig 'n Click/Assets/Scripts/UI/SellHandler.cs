using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellHandler : MonoBehaviour
{
    public AudioClip SellSound;

    private Ore _oreToSell;
    private int _amountSet;
    private int _maxAmount;

    public void Initialize(Ore ore, int maxAmount)
    {
        _oreToSell = ore;
        transform.GetChild(0).Find("Name").gameObject.GetComponent<Text>().text = ore.Name;
        _maxAmount = maxAmount;
        transform.GetChild(0).Find("Slider").gameObject.GetComponent<Slider>().maxValue = _maxAmount;
    }

    public void OnValueChange(float amount)
    {
        _amountSet = (int) amount;
        SetSellButton();
    }

    public void SetMax()
    {
        _amountSet = _maxAmount;
        transform.GetChild(0).Find("Slider").gameObject.GetComponent<Slider>().value = _amountSet;
        SetSellButton();
    }

    private void SetSellButton()
    {
        transform.GetChild(0).Find("Amount").gameObject.GetComponent<Text>().text = _amountSet.ToString();
        transform.GetChild(0).Find("SellButton").GetChild(0).gameObject.GetComponent<Text>().text =
            "+$" + MoneyConverter.ConvertNumber(_amountSet * _oreToSell.Value);
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