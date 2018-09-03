using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    private MovingUIHandler _movingUIHandler;

    private void Awake()
    {
        _movingUIHandler = transform.parent.gameObject.GetComponent<MovingUIHandler>();
    }

    public void Toggle()
    {
        _movingUIHandler.Move(MovingUIHandler.Type.Settings);
    }
}