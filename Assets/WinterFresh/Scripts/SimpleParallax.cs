using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    public Transform[] layers;
    public float horizonalShiftAmount = 0;
    public float verticalShiftAmount = 0;

    private Vector3[] startPositions;

    private void Start()
    {
        startPositions = new Vector3[layers.Length];
        for(int i = 0; i < startPositions.Length; i++)
        {
            startPositions[i] = layers[i].position;
        }
    }

    private void Update()
    {
        //horizonalShiftAmount = Mathf.Sin(Time.time) * 0.3f;
        //verticalShiftAmount = Mathf.Cos(Time.time) * 0.3f;
        for (int i = 0; i < startPositions.Length; i++)
        {
            layers[i].position = startPositions[i] + Vector3.right * horizonalShiftAmount * i + Vector3.up * verticalShiftAmount * i;
        }
    }
}
