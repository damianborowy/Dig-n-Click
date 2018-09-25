using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;
    public Transform EffectsTransform;
    public AudioSource MusicAudioSource;
    public GameObject AudioEffectPrefab;
    public Slider MusicVolumeSlider;
    public Slider EffectsVolumeSlider;

    private float _effectsVolume;
    private float _musicVolume;
    private List<AudioSource> _loopedAudioEffects = new List<AudioSource>();

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

        SetAudioVolume();
    }

    private void SetAudioVolume()
    {
        _musicVolume = MusicAudioSource.volume;
        _effectsVolume = 1;
        LoadPlayerAudioPrefs();
    }

    private void LoadPlayerAudioPrefs()
    {
        if (PlayerPrefs.HasKey("BG"))
        {
            float musicVolume = PlayerPrefs.GetFloat("BG");
            MusicVolumeSlider.value = musicVolume;
        }

        if (PlayerPrefs.HasKey("FX"))
        {
            float effectsVolume = PlayerPrefs.GetFloat("FX");
            EffectsVolumeSlider.value = effectsVolume;
        }
    }

    public void ChangeMusicVolume(float value)
    {
        MusicAudioSource.volume = _musicVolume * value;

        PlayerPrefs.SetFloat("BG", MusicVolumeSlider.value);
    }

    public void ChangeEffectsVolume(float value)
    {
        _effectsVolume = value;

        PlayerPrefs.SetFloat("FX", EffectsVolumeSlider.value);
    }

    public void PlayAudioEffect(AudioClip audioClip, float volume)
    {
        AudioSource audioSource = CreateAudioSource(audioClip, volume);
        audioSource.Play();
        DestroyByClipLength(audioSource);
    }

    public void PlayAudioEffectInLoop(AudioClip audioClip, float volume)
    {
        AudioSource audioSource = CreateAudioSource(audioClip, volume);
        audioSource.loop = true;
        audioSource.Play();
        _loopedAudioEffects.Add(audioSource);
    }

    private AudioSource CreateAudioSource(AudioClip audioClip, float volume)
    {
        GameObject audioEffectGameObject = Instantiate(AudioEffectPrefab, EffectsTransform);
        AudioSource audioSource = audioEffectGameObject.GetComponent<AudioSource>();
        audioSource.volume = volume * _effectsVolume;
        audioSource.clip = audioClip;
        return audioSource;
    }

    public void StopAndDestroyLoopedAudioEffects()
    {
        foreach (var loopedAudioEffect in _loopedAudioEffects)
        {
            loopedAudioEffect.loop = false;
            DestroyByClipLength(loopedAudioEffect);
        }
    }

    private void DestroyByClipLength(AudioSource audioSource)
    {
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}