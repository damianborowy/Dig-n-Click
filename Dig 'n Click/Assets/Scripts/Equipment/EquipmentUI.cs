using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentUI : MonoBehaviour {

    private bool toggled;

	void Start () {
        gameObject.SetActive(false);
        toggled = false;
	}

    public void ToggleActive()
    {
        if (toggled)
        {
            gameObject.SetActive(false);
            toggled = false;
        }
        else
        {
            gameObject.SetActive(true);
            toggled = true;
        }
    }
}
