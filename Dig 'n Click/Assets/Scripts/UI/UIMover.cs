using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMover : MonoBehaviour
{
    public float MoveSpeed;
    public Direction MoveAnchor;

    private RectTransform _rectTransform;
    private RectTransform _canvasRectTransform;
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
        _canvasRectTransform = transform.parent.gameObject.GetComponent<RectTransform>();
        if (MoveAnchor == Direction.Left || MoveAnchor == Direction.Right)
        {
            float canvasWidth = _canvasRectTransform.rect.width * _canvasRectTransform.localScale.x;
            _rectTransform.position = _rectTransform.position + _directionVector3 * canvasWidth;
        }
        else
        {
            float canvasHeight = _canvasRectTransform.rect.height * _canvasRectTransform.localScale.y;
            _rectTransform.position = _rectTransform.position + _directionVector3 * canvasHeight;
        }

        _startPosition = _rectTransform.position;
        _isInside = false;
    }

    public void MoveUI()
    {
        if (_isUIMoving) return;

        _isUIMoving = true;
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
            while (transform.position.y != _canvasRectTransform.position.y) //precision issue to fix
            {
                float newY = Mathf.MoveTowards(transform.position.y, _canvasRectTransform.position.y,
                    MoveSpeed * Time.deltaTime);
                transform.position =
                    new Vector3(transform.position.x, newY, transform.position.z);
                yield return null;
            }
        else
            while (transform.position.x != _canvasRectTransform.position.x) //precision issue to fix
            {
                float newX = Mathf.MoveTowards(transform.position.x, _canvasRectTransform.position.x,
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
            while (transform.position.y != _startPosition.y) //precision issue to fix
            {
                float newY = Mathf.MoveTowards(transform.position.y, _startPosition.y,
                    MoveSpeed * Time.deltaTime);
                transform.position =
                    new Vector3(transform.position.x, newY, transform.position.z);
                yield return null;
            }
        else
            while (transform.position.x != _startPosition.x) //precision issue to fix
            {
                float newX = Mathf.MoveTowards(transform.position.x, _startPosition.x,
                    MoveSpeed * Time.deltaTime);
                transform.position =
                    new Vector3(newX, transform.position.y, transform.position.z);
                yield return null;
            }

        _isUIMoving = false;
        _isInside = false;
    }
}