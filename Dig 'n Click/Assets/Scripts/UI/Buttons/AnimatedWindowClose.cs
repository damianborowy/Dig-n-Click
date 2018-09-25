using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedWindowClose : MonoBehaviour
{
    public int ParentLevel;
    public float TriggerAtPercent;
    public float ScaleSpeed;
    public Mode ButtonMode;
    public AudioClip CloseSound;
    public float CloseSoundVolume;

    public enum Mode
    {
        Destroy,
        SetInactive,
        Hide
    }

    private Button _button;
    private Transform _parent;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _parent = transform;
        for (int i = 0; i < ParentLevel; ++i)
            _parent = _parent.parent;
    }

    public void CloseWindow()
    {
        _button.interactable = false;
        PlayCloseSound();
        StartCoroutine(CloseAnimation());
    }

    private IEnumerator CloseAnimation()
    {
        float startParentScale = _parent.localScale.x;

        while (_parent.localScale.x > startParentScale * TriggerAtPercent)
        {
            float newScale = Mathf.MoveTowards(_parent.localScale.x, 0, ScaleSpeed * Time.deltaTime);
            _parent.localScale = new Vector3(newScale, newScale, newScale);
            yield return null;
        }

        FinalizeButton();
    }

    private void FinalizeButton()
    {
        switch (ButtonMode)
        {
            case Mode.Destroy:
                Destroy(_parent.gameObject);
                break;
            case Mode.SetInactive:
                _parent.gameObject.SetActive(false);
                break;
            case Mode.Hide:
                _parent.localScale = Vector3.zero;
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
    }

    private void PlayCloseSound()
    {
        AudioController.Instance.PlayAudioEffect(CloseSound, CloseSoundVolume);
    }
}