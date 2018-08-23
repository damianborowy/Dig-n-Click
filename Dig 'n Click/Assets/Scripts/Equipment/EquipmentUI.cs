using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public int ColumnCount;
    public int RowCount;
    public GameObject Slot;

    private bool _toggled;
    private int _slotAmount;

    private void Awake()
    {
        _toggled = false;
        _slotAmount = ColumnCount * RowCount;
        for (int i = 0; i < _slotAmount; ++i)
            Instantiate(Slot, transform.Find("Slots Panel"));
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

    public int GetColumnCount()
    {
        return ColumnCount;
    }

    public int GetRowCount()
    {
        return RowCount;
    }
}