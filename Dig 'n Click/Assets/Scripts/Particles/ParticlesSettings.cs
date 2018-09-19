using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesSettings : MonoBehaviour
{
    private void Start()
    {
        if(!PlayerPrefs.HasKey("HIT"))
            PlayerPrefs.SetInt("HIT", 1);
        if (!PlayerPrefs.HasKey("DESTROY"))
            PlayerPrefs.SetInt("DESTROY", 1);
    }

    public void ChangeHitParticlesSetting(bool value)
    {
        PlayerPrefs.SetInt("HIT", value ? 1 : 0);
    }

    public void ChangeDestroyParticlesSetting(bool value)
    {
        PlayerPrefs.SetInt("DESTROY", value ? 1 : 0);
    }
}