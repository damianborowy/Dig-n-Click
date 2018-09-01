using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellButtonController : MonoBehaviour
{
    public GameObject SellPanel;
    public float SlotXSizePercent;
    public float SlotYSizePercent;

    private RectTransform _rectTransform;
    private Ore _assignedOre;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x * SlotXSizePercent,
            _rectTransform.sizeDelta.y * SlotYSizePercent);
    }

    public void OnClick()
    {
        GameObject sellPanelGameObject = Instantiate(SellPanel, GameObject.FindWithTag("Canvas").transform);
        SellHandler sellHandler = sellPanelGameObject.GetComponent<SellHandler>();
        sellHandler.Initialize(_assignedOre, EquipmentController.Instance.Items[_assignedOre]);
        Clear();
    }

    public void SetParent(RectTransform parent)
    {
        _rectTransform.SetParent(parent);
        Vector3[] parentCorners = new Vector3[4];
        parent.GetWorldCorners(parentCorners);
        _rectTransform.position = parentCorners[0];
    }

    public void AssignOreToSell(Ore ore)
    {
        if (_assignedOre == null || _assignedOre != ore)
        {
            _assignedOre = ore;
            gameObject.SetActive(true);
        }
        else
        {
            _assignedOre = null;
            gameObject.SetActive(false);
        }
    }

    private SlotController GetAssignedSlot()
    {
        return _rectTransform.parent.gameObject.GetComponent<SlotController>();
    }

    public void UpdatePosition(List<SlotController> slots)
    {
        if (GetAssignedSlot() == null) return;
        if (GetAssignedSlot().GetAssignedOre() == _assignedOre) return;

        foreach (var element in slots)
        {
            if (element.GetAssignedOre() == _assignedOre)
            {
                SetParent(element.gameObject.GetComponent<RectTransform>());
                break;
            }
        }
    }

    public void Clear()
    {
        _assignedOre = null;
        SetParent(_rectTransform.parent.GetComponent<RectTransform>());
        gameObject.SetActive(false);
    }
}