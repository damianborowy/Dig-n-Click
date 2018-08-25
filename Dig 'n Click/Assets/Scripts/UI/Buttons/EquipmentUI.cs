using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    public GameObject Slot;
    
    private MovingUIHandler _movingUIHandler;

    private void Awake()
    {
        _movingUIHandler = transform.parent.gameObject.GetComponent<MovingUIHandler>();
        for (int i = 0; i < EquipmentController.Instance.Capacity; ++i)
        {
            GameObject slot = Instantiate(Slot, transform.Find("ScrollableSlots").Find("SlotsPanel"));
            EquipmentController.Instance.AddItemSlot(slot);
        }
    }

    public void Toggle()
    {
        _movingUIHandler.Move(MovingUIHandler.Type.Equipment);
    }
}