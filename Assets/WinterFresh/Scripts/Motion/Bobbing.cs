using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobbing : MonoBehaviour
{
    public float verticalShiftAmount = 0.5f;
    public float rotationShiftAmount = 0.5f;
    public float shiftSpeed = 0.5f;
    private Vector3 basePosition;
    private Vector3 baseUp;
    private float timeAccum = 0;

    private void OnEnable()
    {
        basePosition = transform.localPosition;
        baseUp = transform.up;
    }

    private void Update()
    {
        timeAccum += Time.deltaTime * shiftSpeed;
        float smallSine = Mathf.Sin(timeAccum * Mathf.PI * 16);
        float mediumSine = Mathf.Sin(timeAccum * Mathf.PI * 8);
        float largeSine = Mathf.Sin(timeAccum * Mathf.PI * 4);
        float slowSine = Mathf.Sin(timeAccum * Mathf.PI * 2);

        float positionSine = (smallSine + mediumSine + largeSine) * 0.333f;
        float rotationSine = (slowSine + mediumSine - largeSine) * 0.333f;

        transform.localPosition = basePosition - Vector3.up * positionSine * verticalShiftAmount;
        // transform.up = baseUp + Vector3.right * (rotationSine + slowSine * 0.1f) * rotationShiftAmount + Vector3.forward * (rotationSine + slowSine * 0.15777f) * rotationShiftAmount;
        transform.Rotate((smallSine - mediumSine) * rotationShiftAmount, 0, (smallSine - largeSine) * rotationShiftAmount, Space.Self);
    }
}
