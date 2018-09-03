using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    private AudioSource[] _effects;
    private AudioSource _music;
    private float[] _effectsVolume = new float[9];
    private float _musicVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _effects = transform.GetChild(0).gameObject.GetComponents<AudioSource>();
        _music = transform.GetChild(1).gameObject.GetComponent<AudioSource>();

        for (int i = 0; i < 9; ++i)
        {
            _effectsVolume[i] = _effects[i].volume;
        }

        _musicVolume = _music.volume;
    }

    public void ChangeMusicVolume(float value)
    {
        _music.volume = _musicVolume * value;
    }

    public void ChangeEffectsVolume(float value)
    {
        for(int i = 0; i<9; ++i)
        {
            _effects[i].volume = _effectsVolume[i] * value;
        }
    }

    public void PlayPickaxeSound(AudioClip audioClip)
    {
        _effects[0].clip = audioClip;
        _effects[0].Play();
    }

    public void PlayDestroySound(AudioClip audioClip)
    {
        _effects[1].clip = audioClip;
        _effects[1].Play();
    }

    public void PlayGroundHitSound(AudioClip audioClip)
    {
        _effects[2].clip = audioClip;
        _effects[2].Play();
    }

    public void PlayOpenWindowSound(AudioClip audioClip)
    {
        _effects[3].clip = audioClip;
        _effects[3].Play();
    }

    public void PlayCloseWindowSound(AudioClip audioClip)
    {
        _effects[4].clip = audioClip;
        _effects[4].Play();
    }

    public void PlayLevelUpSound(AudioClip audioClip)
    {
        _effects[5].clip = audioClip;
        _effects[5].Play();
    }

    public void PlayLevelDownSound(AudioClip audioClip)
    {
        _effects[6].clip = audioClip;
        _effects[6].Play();
    }

    public void PlayOredropSound(AudioClip audioClip)
    {
        _effects[7].clip = audioClip;
        _effects[7].Play();
    }

    public void PlayBuySellSound(AudioClip audioClip)
    {
        _effects[8].clip = audioClip;
        _effects[8].Play();
    }
}