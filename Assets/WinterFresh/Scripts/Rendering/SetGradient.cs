using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetGradient : MonoBehaviour
{
    public Gradient gradient;
    public string gradientName = "_Gradient";
    public int gradientRes = 512;

    private Texture2D _gradientTexture;
    private Material _material;

    private void OnEnable()
    {
        _gradientTexture = new Texture2D(gradientRes, 1);
        _material = GetComponent<MeshRenderer>().sharedMaterial;
        UpdateGradientTexture();
    }

    private void UpdateGradientTexture()
    {
        if(_gradientTexture == null) return;
        for(int i = 0; i < gradientRes; i++)
        {
            _gradientTexture.SetPixel(i,0,gradient.Evaluate((float)i/(float)gradientRes));
        }
        _gradientTexture.Apply();
        _material.SetTexture(gradientName, _gradientTexture);
    }

    private void OnValidate() 
    {
        UpdateGradientTexture();
    }
}
