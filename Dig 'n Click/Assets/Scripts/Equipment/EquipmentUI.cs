using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public int SlotAmount;
    public Transform SlotsPanel;
    public GameObject Slot;

    private bool _toggled;

    void Awake()
    {
        _toggled = false;
        for (int i = 0; i < SlotAmount; ++i)
        {
            Instantiate(Slot, SlotsPanel);
        }
    }

    public void ToggleActive()
    {
        if (_toggled)
        {
            gameObject.SetActive(false);
            _toggled = false;
        }
        else
        {
            gameObject.SetActive(true);
            _toggled = true;
        }
    }
}