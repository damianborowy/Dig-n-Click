using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentUI : MonoBehaviour
{
    public GameObject Slot;

    private MovingUIHandler _movingUIHandler;

    private void Awake()
    {
        _movingUIHandler = transform.parent.gameObject.GetComponent<MovingUIHandler>();
        for (int i = 0; i < EquipmentController.Instance.Capacity; ++i)
        {
            GameObject slotGameObject = Instantiate(Slot, transform.Find("ScrollableSlots").Find("SlotsPanel"));
            SlotController slotController = slotGameObject.GetComponent<SlotController>();
            EquipmentController.Instance.AddItemSlot(slotController);
        }
    }

    public void Toggle()
    {
        _movingUIHandler.Move(MovingUIHandler.Type.Equipment);

        GameObject sellButtonGameObject = GameObject.FindWithTag("SellButton");
        if (sellButtonGameObject != null)
            sellButtonGameObject.GetComponent<SellButtonController>().Clear();
    }
}