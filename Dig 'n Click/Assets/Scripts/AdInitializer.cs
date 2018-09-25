using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdInitializer : MonoBehaviour
{
    public string AppID;

    void Start()
    {
        Advertisement.Initialize(AppID);
    }
}