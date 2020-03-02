using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPointDir : MonoBehaviour
{
    public Light dirLight;
    public Transform A;
    public Transform B;
    public bool brighterCloser = false;
    public float closerDistance = 1;

    private float initialBrightness;

    private void Start()
    {
        initialBrightness = dirLight.intensity;
    }

    private void Update()
    {

        transform.forward = (B.position - A.position).normalized;

        float dist = Vector3.Distance(B.position, A.position);
        if (dist > closerDistance) return;

        if (brighterCloser) dirLight.intensity = initialBrightness * (1 - (dist / closerDistance));
        else dirLight.intensity = initialBrightness * (dist / closerDistance);

    }
}
