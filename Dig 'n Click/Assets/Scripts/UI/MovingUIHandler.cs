using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MovingUIHandler : MonoBehaviour
{
    public UIMover Equipment;
    public UIMover Upgrades;
   
    public enum Type
    {
        Equipment, Upgrades
    }

    private bool _isElementInside;
    private UIMover _elementInside;

    private void Awake()
    {
        _isElementInside = false;
    }

    public void Move(Type type)
    {
        if (Equipment.IsUIMoving() || Upgrades.IsUIMoving()) return;

        UIMover target;
        switch (type)
        {
            case Type.Equipment:
                target = Equipment;
                break;
            case Type.Upgrades:
                target = Upgrades;
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
