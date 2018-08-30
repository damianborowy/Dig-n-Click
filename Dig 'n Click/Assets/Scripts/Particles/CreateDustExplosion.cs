using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDustExplosion : MonoBehaviour
{
    public GameObject DustExplosion;
    public float Offset;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Instantiate(DustExplosion, transform.position + new Vector3(0, Offset, 0), Quaternion.identity);
    }
}