using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDustExplosion : MonoBehaviour
{
    public GameObject DustExplosion;
    public Transform ParticlesSpawn;
    public float Offset;
    public AudioClip[] GroundHitSound;
    public float GroundHitSoundVolume;

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayGroundHitSound();
        InstantiateDustExplosion();
    }

    private void PlayGroundHitSound()
    {
        AudioClip randomGroundHitSound = GroundHitSound[Random.Range(0, GroundHitSound.Length)];
        AudioController.Instance.PlayAudioEffect(randomGroundHitSound, GroundHitSoundVolume);
    }

    private void InstantiateDustExplosion()
    {
        Vector3 targetPosition = transform.position + new Vector3(0, Offset, 0);
        GameObject instantiatedDustExplosionGameObject = Instantiate(DustExplosion, ParticlesSpawn);
        instantiatedDustExplosionGameObject.transform.position = targetPosition;
    }
}