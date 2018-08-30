using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    public float TimeToDestroy;

    private void Start()
    {
        Destroy(gameObject, TimeToDestroy);
    }
}