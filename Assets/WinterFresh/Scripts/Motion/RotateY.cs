using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateY : MonoBehaviour
{
    public float rotateSpeed = 10;
    private void Update()
    {
        transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f, Space.Self);
    }
}
