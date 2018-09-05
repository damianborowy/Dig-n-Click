using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentUI : MonoBehaviour
{
    public GameObject Slot;
    public GameObject ScrollableSlots;
    public GameObject PrestigeSlots;
    public Button OresBookmark;
    public Button PrestigeBookmark;

    private MovingUIHandler _movingUIHandler;

    private void Awake()
    {
        _movingUIHandler = transform.parent.gameObject.GetComponent<MovingUIHandler>();
        for (int i = 0; i < EquipmentController.Instance.Capacity; ++i)
        {
            GameObject slotGameObject = Instantiate(Slot, ScrollableSlots.transform.Find("SlotsPanel"));
            SlotController slotController = slotGameObject.GetComponent<SlotController>();
            EquipmentController.Instance.AddItemSlot(slotController);
        }
    }

    private void Start()
    {
        ChangeToOres();
    }

    public void ChangeToOres()
    {
        ScrollableSlots.SetActive(true);
        PrestigeSlots.SetActive(false);
        OresBookmark.interactable = false;
        PrestigeBookmark.interactable = true;
    }

    public void ChangeToPrestige()
    {
        PrestigeSlots.SetActive(true);
        ScrollableSlots.SetActive(false);
        PrestigeBookmark.interactable = false;
        OresBookmark.interactable = true;
    }

    public void Toggle()
    {
        _movingUIHandler.Move(MovingUIHandler.Type.Equipment);

        GameObject sellButtonGameObject = GameObject.FindWithTag("SellButton");
        if (sellButtonGameObject != null)
            sellButtonGameObject.GetComponent<SellButtonController>().Clear();
    }
}