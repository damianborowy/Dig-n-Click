using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFader : MonoBehaviour
{
    public float FadeSpeed;

    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        while (_text.color.a > 0)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b,
                Mathf.MoveTowards(_text.color.a, 0, FadeSpeed * Time.deltaTime));
            yield return null;
        }
        Destroy(_text.gameObject);
    }
}