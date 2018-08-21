using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActivator : MonoBehaviour
{
    private Color _full = new Color(255, 255, 255, 1);
    private Color _faded = new Color(255, 255, 255, 0.9f);
    private Button _b;

    private void Start()
    {
        _b = GetComponent<Button>();
    }

    public void SetActive()
    {
        _b.image.color = _full;
        _b.interactable = true;
    }

    public void SetInactive()
    {
        _b.image.color = _faded;
        _b.interactable = false;
    }
}