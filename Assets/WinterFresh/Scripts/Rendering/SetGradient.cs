using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetGradient : MonoBehaviour
{
    public Gradient gradientA;
    public Gradient gradientB;
    public int gradientRes = 512;

    private Texture2D _gradientTextureA;
    private Texture2D _gradientTextureB;
    private Material _material;

    private void OnEnable()
    {
        _gradientTextureA = new Texture2D(gradientRes, 1);
        _gradientTextureB = new Texture2D(gradientRes, 1);
        _material = GetComponent<MeshRenderer>().sharedMaterial;

        UpdateGradientTexture("_GradientA", gradientA, _gradientTextureA);
        UpdateGradientTexture("_GradientB", gradientB, _gradientTextureB);
    }

    private void UpdateGradientTexture(string gradientName, Gradient gradient, Texture2D _gradientTexture)
    {
        if(_gradientTexture == null) return;
        for(int i = 0; i < gradientRes; i++)
        {
            _gradientTexture.SetPixel(i,0,gradient.Evaluate((float)i/(float)gradientRes));
        }
        _gradientTexture.Apply();
        _material.SetTexture(gradientName, _gradientTexture);
    }
}
