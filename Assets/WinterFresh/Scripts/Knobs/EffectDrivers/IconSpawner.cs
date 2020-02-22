using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSpawner : Observer<NetworkIntMessage>
{
    public Transform suckTarget;

    public Transform[] spawnPoints;

    public GameObject iconCarrier;

    public Texture[][] knobs;

    public bool activated = false;


    public override void Observed(ref NetworkIntMessage msg)
    {
        if(msg.MsgType == NetworkIntMessage.MessageType.STATE)
        {
            activated = msg.data[1] == 1 ? true : false;
        }

        if(msg.MsgType == NetworkIntMessage.MessageType.KNOBS)
        {
            int knobId = msg.data[1];
            int knobVal = msg.data[2];
            //Texture t = knobs[knobId][knobVal/16];

           GameObject g = GameObject.Instantiate(iconCarrier, spawnPoints[knobId].position, Quaternion.identity);
           //g.GetComponent<MeshRenderer>().material.SetTexture("_UnlitColorMap", t);
           g.GetComponent<BeamSuckedObject>().suckTarget = suckTarget;
        }
    }
}
