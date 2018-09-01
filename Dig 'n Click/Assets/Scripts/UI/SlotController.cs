using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float DragTolerance;

    private RectTransform _rectTransform;
    private Ore _assignedOre;
    private SellButtonController _sellButtonController;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        ClearSlot();
    }

    private void Start()
    {
        _sellButtonController = GameObject.FindWithTag("SellButton").GetComponent<SellButtonController>();
    }

    public void AssignOre(Ore ore, int amount)
    {
        _assignedOre = ore;

        GameObject itemImageGameObject = transform.Find("Item").gameObject;
        itemImageGameObject.SetActive(true);
        Image itemImage = itemImageGameObject.GetComponent<Image>();
        itemImage.sprite = ore.OreSprite;
        GameObject amountInSlotGameObject = transform.Find("Amount").gameObject;
        amountInSlotGameObject.SetActive(true);
        Text itemAmount = amountInSlotGameObject.GetComponent<Text>();
        itemAmount.text = amount.ToString();
    }

    public Ore GetAssignedOre()
    {
        return _assignedOre;
    }

    public void UpdateOre(int amount)
    {
        GameObject amountInSlot = transform.Find("Amount").gameObject;
        Text itemAmount = amountInSlot.GetComponent<Text>();
        itemAmount.text = amount.ToString();
    }

    public void ClearSlot()
    {
        _assignedOre = null;
        _rectTransform.Find("Item").gameObject.SetActive(false);
        _rectTransform.Find("Amount").gameObject.SetActive(false);
    }

    public bool IsEmpty()
    {
        return _assignedOre == null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if ((eventData.position - eventData.pressPosition).magnitude >
            _rectTransform.rect.width * DragTolerance) return;

        if (!IsEmpty())
        {
            _sellButtonController.SetParent(_rectTransform);
            _sellButtonController.AssignOreToSell(_assignedOre);
        }
        else
        {
            _sellButtonController.Clear();
        }
    }
}