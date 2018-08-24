using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSave : MonoBehaviour {

    public int SaveInterval = 5;

	void Start () {
	    if (GameController.Instance.EnableSaving)
	    {
	        StartCoroutine(AutoSaveCoroutine());
	    }
	}
	
    IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(SaveInterval);

            GameController.Instance.Save();
        }
    }
}