using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RockOnClick : MonoBehaviour, IPointerDownHandler
{
    private RockController _rc;

    private void Start()
    {
        _rc = GetComponent<RockController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _rc.Hit(GameController.Instance.GetStrength());
    }
}