using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivator : MonoBehaviour
{
    private Color _full = new Color(255, 255, 255, 1);
    private Color _faded = new Color(255, 255, 255, 0.9f);
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    public void SetActive()
    {
        _button.image.color = _full;
        _button.interactable = true;
    }

    public void SetInactive()
    {
        _button.image.color = _faded;
        _button.interactable = false;
    }

    public void Hide()
    {
        _button.transform.localScale = new Vector3(0, 0, 0);
    }

    public void Show()
    {
        _button.transform.localScale = new Vector3(1, 1, 1);
    }
}