using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float ScrollSpeed;
    public float WaitTime;

    private Vector3 _startPosition;
    private float _tileYLength;
    private Renderer _meshRenderer;
    private bool _isBackgroundScrolling;

    private void Start()
    {
        _startPosition = transform.position;
        _meshRenderer = GetComponent<Renderer>();
        _tileYLength = _meshRenderer.bounds.size.y;
        _isBackgroundScrolling = false;
    }

    private IEnumerator Scroll()
    {
        while (transform.position.y != _startPosition.y + _tileYLength)
        {
            float newY = Mathf.MoveTowards(transform.position.y, _startPosition.y + _tileYLength, ScrollSpeed);
            transform.position = _startPosition + newY * Vector3.up;
            yield return new WaitForSeconds(WaitTime);
        }

        transform.position = _startPosition;
        _isBackgroundScrolling = false;
    }

    public void ScrollBackground()
    {
        if (!_isBackgroundScrolling)
        {
            _isBackgroundScrolling = true;
            StartCoroutine(Scroll());
        }
    }

    public bool IsBackgroundScrolling()
    {
        return _isBackgroundScrolling;
    }
}