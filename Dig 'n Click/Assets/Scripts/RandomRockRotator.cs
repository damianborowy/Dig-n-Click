using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockRotator : MonoBehaviour
{
    public float RotationSpeed;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.angularVelocity = Random.value * RotationSpeed;
    }
}