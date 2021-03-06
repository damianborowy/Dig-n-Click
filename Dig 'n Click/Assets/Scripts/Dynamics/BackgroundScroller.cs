﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float ScrollSpeed;
    public int ComaprisonPrecisionExponent;

    private Vector3 _startPosition;
    private float _tileYLength;
    private Renderer _meshRenderer;
    private bool _isBackgroundScrolling;

    private void Start()
    {
        _meshRenderer = GetComponent<Renderer>();
        _tileYLength = _meshRenderer.bounds.size.y;
        _isBackgroundScrolling = false;
    }

    public void ScrollBackground(Direction direction)
    {
        if (_isBackgroundScrolling) return;

        _isBackgroundScrolling = true;

        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particles");
        foreach (var element in particles)
            Destroy(element);

        if (direction == Direction.Up)
            StartCoroutine(ScrollUp());
        else if (direction == Direction.Down)
            StartCoroutine(ScrollDown());
        else
            throw new InvalidEnumArgumentException();
    }

    public bool IsBackgroundScrolling()
    {
        return _isBackgroundScrolling;
    }

    private IEnumerator ScrollDown()
    {
        _startPosition = transform.position;

        while ((int) (transform.position.y * 10 * ComaprisonPrecisionExponent) !=
               (int) ((_startPosition.y + _tileYLength) * 10 * ComaprisonPrecisionExponent)
        ) //rounding values to avoid float comparison
        {
            float newY = Mathf.MoveTowards(transform.position.y, _startPosition.y + _tileYLength,
                ScrollSpeed * Time.deltaTime);
            transform.position = newY * Vector3.up;
            yield return null;
        }

        transform.position = _startPosition;

        _isBackgroundScrolling = false;
    }

    private IEnumerator ScrollUp()
    {
        _startPosition = transform.position + _tileYLength * Vector3.up;

        transform.position = _startPosition;

        while ((int) ((transform.position.y + _tileYLength) * 10 * ComaprisonPrecisionExponent) !=
               (int) (_startPosition.y * 10 * ComaprisonPrecisionExponent)) //rounding values to avoid float comparison
        {
            float newY = Mathf.MoveTowards(transform.position.y, _startPosition.y - _tileYLength,
                ScrollSpeed * Time.deltaTime);
            transform.position = newY * Vector3.up;
            yield return null;
        }

        _isBackgroundScrolling = false;
    }
}