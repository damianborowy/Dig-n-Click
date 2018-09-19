using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellHandler : MonoBehaviour
{
    public AudioClip SellSound;
    public float SellSoundVolume;

    private Ore _oreToSell;
    private int _amountSet;
    private int _maxAmount;
    private Text _oreName;
    private Slider _amountSlider;

    public void Initialize(Ore ore, int maxAmount)
    {
        _oreToSell = ore;
        _oreName = transform.GetChild(0).Find("Name").gameObject.GetComponent<Text>();
        _oreName.text = ore.Name;
        _maxAmount = maxAmount;
        _amountSlider = transform.GetChild(0).Find("Slider").gameObject.GetComponent<Slider>();
        _amountSlider.maxValue = _maxAmount;
    }

    public void OnValueChange(float amount)
    {
        _amountSet = (int) amount;
        SetSellButton();
    }

    public void SetMax()
    {
        _amountSet = _maxAmount;
        _amountSlider.value = _amountSet;
        SetSellButton();
    }

    public void UpdateMaxAmount()
    {
        if (_oreToSell == null) return;
        _maxAmount = EquipmentController.Instance.Items[_oreToSell];
        _amountSlider.maxValue = _maxAmount;
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
            AudioController.Instance.PlayAudioEffect(SellSound, SellSoundVolume);
            GameController.Instance.AddMoney(_oreToSell.Value * _amountSet);
        }

        _oreToSell = null;
    }
}