using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MaterialPropertyDriver : Observer<NetworkIntMessage>
{    
    public MaterialElement[] targetProperties;

    private bool _activated = false;

    [Serializable]
    public class MaterialElement
    {
        public Material material;
        public string propertyName;
        public int knobId = 0;
        public float bottomOfRange;
        public float topOfRange;
        [HideInInspector]
        public float smoothedValue;
        [HideInInspector]
        public float targetValue;

    }

    private void Update() 
    {
        foreach(MaterialElement me in targetProperties)
        {
            me.smoothedValue = Mathf.Lerp(me.smoothedValue, Mathf.Lerp(me.bottomOfRange, me.topOfRange, me.targetValue), Time.deltaTime * 6);
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
            foreach(MaterialElement me in targetProperties)
            {
                if(me.knobId == msg.data[1])
                {
                    me.targetValue = msg.data[2]/16f;
                }
            }
        }
    }
}
