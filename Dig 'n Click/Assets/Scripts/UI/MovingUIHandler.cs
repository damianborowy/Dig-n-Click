using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MovingUIHandler : MonoBehaviour
{
    public UIMover Equipment;
    public UIMover Upgrades;
    public UIMover Settings;
    public float DoubleClickInterval;

    public enum Type
    {
        Equipment,
        Upgrades,
        Settings
    }

    private bool _isElementInside;
    private UIMover _elementInside;
    private bool _clickedOnce = false;

    private void Awake()
    {
        _isElementInside = false;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        if (Equipment.IsUIMoving() || Upgrades.IsUIMoving() || Settings.IsUIMoving()) return;

        if (_isElementInside)
        {
            _elementInside.MoveUI();
            _isElementInside = false;
        }
        else if (!_clickedOnce)
        {
            _clickedOnce = true;
            StartCoroutine(ResetClickCount());
        }
        else
        {
            Application.Quit();
        }
    }

    private IEnumerator ResetClickCount()
    {
        yield return new WaitForSeconds(DoubleClickInterval);
        _clickedOnce = false;
    }

    public void Move(Type type)
    {
        if (Equipment.IsUIMoving() || Upgrades.IsUIMoving() || Settings.IsUIMoving()) return;

        UIMover target;
        switch (type)
        {
            case Type.Equipment:
                target = Equipment;
                break;
            case Type.Upgrades:
                target = Upgrades;
                break;
            case Type.Settings:
                target = Settings;
                break;
            default:
                throw new InvalidEnumArgumentException();
        }

        if (_isElementInside)
        {
            if (target == _elementInside)
                _isElementInside = false;
            else
            {
                _elementInside.MoveUI();
                _elementInside = target;
            }
        }
        else
        {
            _elementInside = target;
            _isElementInside = true;
        }

        target.MoveUI();
    }
}