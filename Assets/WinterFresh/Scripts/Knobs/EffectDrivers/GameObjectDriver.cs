using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDriver : Observer<NetworkIntMessage>
{
    public int knobId;
    public Color A;
    public Color B;
    public float blendTime = 10;
    private Material _material;
    private Color _colorTarget;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
	    Color originalColor = _material.GetColor("_BaseColor");
        _material.SetColor("_BaseColor", Color.Lerp(originalColor, _colorTarget, Time.deltaTime * blendTime));
    }

    public override void Observed(ref NetworkIntMessage msg)
    {
        if(msg.MsgType == NetworkIntMessage.MessageType.KNOBS) 
        {
            _colorTarget = Color.Lerp(A, B, msg.data[knobId] / 64f);
        } 
    }
}
