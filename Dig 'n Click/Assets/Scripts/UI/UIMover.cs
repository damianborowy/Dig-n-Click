using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMover : MonoBehaviour
{
    public float MoveSpeed;
    public float ComparisonPrecisionExponent;
    public Direction MoveAnchor;

    private RectTransform _rectTransform;
    private RectTransform _parentRectTransform;
    private Vector3 _directionVector3;
    private Vector3 _startPosition;
    private bool _isUIMoving;
    private bool _isInside;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        switch (MoveAnchor)
        {
            case Direction.Up:
                _directionVector3 = Vector3.up;
                break;
            case Direction.Down:
                _directionVector3 = Vector3.down;
                break;
            case Direction.Left:
                _directionVector3 = Vector3.left;
                break;
            case Direction.Right:
                _directionVector3 = Vector3.right;
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
    }

    private void Start()
    {
        _parentRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();
        if (MoveAnchor == Direction.Left || MoveAnchor == Direction.Right)
        {
            float parentWidth = _parentRectTransform.rect.width * _parentRectTransform.lossyScale.x;
            _rectTransform.position = _parentRectTransform.position + _directionVector3 * parentWidth;
        }
        else
        {
            float parentHeight = _parentRectTransform.rect.height * _parentRectTransform.lossyScale.y;
            _rectTransform.position = _parentRectTransform.position + _directionVector3 * parentHeight;
        }

        _startPosition = _rectTransform.position;
        _isInside = false;
        gameObject.SetActive(false);
    }

    public void MoveUI()
    {
        if (_isUIMoving) return;

        _isUIMoving = true;
        gameObject.SetActive(true);

        if (_isInside)
            StartCoroutine(MoveOutside());
        else
            StartCoroutine(MoveInside());
    }

    public bool IsUIMoving()
    {
        return _isUIMoving;
    }

    private IEnumerator MoveInside()
    {
        if (MoveAnchor == Direction.Up || MoveAnchor == Direction.Down)
            while ((int) (transform.position.y * 10 * ComparisonPrecisionExponent) !=
                   (int) (_parentRectTransform.position.y * 10 * ComparisonPrecisionExponent)) //rounding values to avoid float comparison
            {
                float newY = Mathf.MoveTowards(transform.position.y, _parentRectTransform.position.y,
                    MoveSpeed * Time.deltaTime);
                transform.position =
                    new Vector3(transform.position.x, newY, transform.position.z);
                yield return null;
            }
        else
            while ((int) (transform.position.x * 10 * ComparisonPrecisionExponent) !=
                   (int) (_parentRectTransform.position.x * 10 * ComparisonPrecisionExponent)) //rounding values to avoid float comparison
            {
                float newX = Mathf.MoveTowards(transform.position.x, _parentRectTransform.position.x,
                    MoveSpeed * Time.deltaTime);
                transform.position =
                    new Vector3(newX, transform.position.y, transform.position.z);
                yield return null;
            }

        _isUIMoving = false;
        _isInside = true;
    }

    private IEnumerator MoveOutside()
    {
        if (MoveAnchor == Direction.Up || MoveAnchor == Direction.Down)
            while ((int) (transform.position.y * 10 * ComparisonPrecisionExponent) !=
                   (int) (_startPosition.y * 10 * ComparisonPrecisionExponent)) //rounding values to avoid float comparison
            {
                float newY = Mathf.MoveTowards(transform.position.y, _startPosition.y,
                    MoveSpeed * Time.deltaTime);
                transform.position =
                    new Vector3(transform.position.x, newY, transform.position.z);
                yield return null;
            }
        else
            while ((int) (transform.position.x * 10 * ComparisonPrecisionExponent) !=
                   (int) (_startPosition.x * 10 * ComparisonPrecisionExponent)) //rounding values to avoid float comparison
            {
                float newX = Mathf.MoveTowards(transform.position.x, _startPosition.x,
                    MoveSpeed * Time.deltaTime);
                transform.position =
                    new Vector3(newX, transform.position.y, transform.position.z);
                yield return null;
            }

        _isUIMoving = false;
        _isInside = false;

        gameObject.SetActive(false);
    }
}