using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindParticleCollider : MonoBehaviour
{
    private void Start()
    {
        GetComponent<ParticleSystem>().collision.SetPlane(0, GameObject.FindWithTag("Ground").transform);
    }
}