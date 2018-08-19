using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundColliderPositioner : MonoBehaviour {
    private void Awake()
    {
        double worldScreenHeight = Camera.main.orthographicSize * 2.0;

        transform.position = new Vector3(transform.position.x, -(float)worldScreenHeight/10, transform.position.z);
    }
}
