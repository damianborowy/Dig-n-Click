using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class TextMover : MonoBehaviour
{
    public float MoveSpeed;
    public Direction MoveDirection;

    private RectTransform _textRectTransform;

    private void Start()
    {
        _textRectTransform = GetComponent<RectTransform>();
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            Vector3 directionVector3;
            switch (MoveDirection)
            {
                case Direction.Up:
                    directionVector3 = Vector3.up;
                    break;
                case Direction.Down:
                    directionVector3 = Vector3.down;
                    break;
                case Direction.Right:
                    directionVector3 = Vector3.right;
                    break;
                case Direction.Left:
                    directionVector3 = Vector3.left;
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }

            _textRectTransform.position += directionVector3 * MoveSpeed * Time.deltaTime;
            yield return null;
        }
    }
}