using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OresList : MonoBehaviour {

    public static OresList Instance;
    public List<Ore> Ores;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
