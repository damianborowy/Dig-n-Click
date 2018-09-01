using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDustExplosion : MonoBehaviour
{
    public GameObject DustExplosion;
    public float Offset;
    public AudioClip[] GroundHitSound;

    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioController.Instance.PlayGroundHitSound(GroundHitSound[Random.Range(0, GroundHitSound.Length)]);
        Instantiate(DustExplosion, transform.position + new Vector3(0, Offset, 0), Quaternion.identity);
    }
}