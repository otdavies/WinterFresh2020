using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderQueue : MonoBehaviour
{
    public int queue;
    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = queue;
    }
}
