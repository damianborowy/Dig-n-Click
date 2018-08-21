using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowResizer : MonoBehaviour
{
    public float ResizeSpeed;

    private Vector3 _startScale;

    private void Awake()
    {
        _startScale = transform.localScale;
    }

    public void Resize()
    {
        transform.localScale = new Vector3(0, 0, transform.localScale.z);
        StartCoroutine(ResizeShadow());
    }

    private IEnumerator ResizeShadow()
    {
        while (transform.localScale != _startScale)
        {
            float newScale = Mathf.MoveTowards(transform.localScale.x, _startScale.x,
                ResizeSpeed * Time.deltaTime);
            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
            yield return null;
        }
    }
}