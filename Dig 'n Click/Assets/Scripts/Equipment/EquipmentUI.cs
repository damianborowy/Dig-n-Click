using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject Slot;

    private bool _toggled;
    private UIMover _uiMover;

    private void Awake()
    {
        _toggled = false;
        for (int i = 0; i < EquipmentController.Instance.Capacity; ++i)
        {
            GameObject slot = Instantiate(Slot, transform.Find("ScrollableSlots").Find("SlotsPanel"));
            EquipmentController.Instance.AddItemSlot(slot);
        }
    }

    private void Start()
    {
        _uiMover = GetComponent<UIMover>();
    }

    public void Toggle()
    {
        if (_toggled && !_uiMover.IsUIMoving())
        {
            Debug.Log("Moving outside");
            _uiMover.MoveUI();
            _toggled = false;
        }
        else if (!_toggled && !_uiMover.IsUIMoving())
        {
            Debug.Log("Moving inside");
            _uiMover.MoveUI();
            _toggled = true;
        }
        else
        {
            Debug.Log("If statement failure");
            Debug.Log("UI moving is: " + _uiMover.IsUIMoving());
        }
    }
}