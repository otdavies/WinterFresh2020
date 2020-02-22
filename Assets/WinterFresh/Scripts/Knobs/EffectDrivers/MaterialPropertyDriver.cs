using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaterialPropertyDriver : Observer<NetworkIntMessage>
{    
    public MaterialElement[] targetProperties;
    public int knobId = 0;

    private float _targetValue = 0;

    private bool _activated = false;

    [Serializable]
    public class MaterialElement
    {
        public Material material;
        public string propertyName;
        public float bottomOfRange;
        public float topOfRange;
        public float smoothedValue;
    }

    private void Update() 
    {
        foreach(MaterialElement me in targetProperties)
        {
            me.smoothedValue = Mathf.Lerp(me.smoothedValue, me.bottomOfRange + (_targetValue * me.topOfRange), Time.deltaTime * 10);
            me.material.SetFloat(me.propertyName, me.smoothedValue);
        }
    }

    public override void Observed(ref NetworkIntMessage msg)
    {
        if(msg.MsgType == NetworkIntMessage.MessageType.STATE)
        {
            _activated = msg.data[1] == 1 ? true : false;
        }

        if(!_activated) return;

        if(msg.MsgType == NetworkIntMessage.MessageType.KNOBS)
        {
            if(knobId == msg.data[1])
            {
                _targetValue = msg.data[2]/64f;
            }
        }
    }
}
