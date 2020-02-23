using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconSpawner : Observer<NetworkIntMessage>
{
    public Transform suckTarget;

    public Transform[] spawnPoints;

    public GameObject iconCarrier;

    public Texture2D[] icons;

    public bool activated = false;

    private int lastSpawned = 0;

    public override void Observed(ref NetworkIntMessage msg)
    {
        if(msg.MsgType == NetworkIntMessage.MessageType.STATE)
        {
            activated = msg.data[1] == 1 ? true : false;
        }

        if(msg.MsgType == NetworkIntMessage.MessageType.KNOBS)
        {
            if (!activated) return;

            int knobId = msg.data[1];
            int knobVal = msg.data[2];
            
            int correctedID = Mathf.RoundToInt(Mathf.Lerp(0, 4, 1 - (knobVal / 14f)));
            
            int spawnID = (knobId * 5) + correctedID;

            if (spawnID != lastSpawned)
            {
                Debug.Log(spawnID);
                lastSpawned = spawnID;
                GameObject g = GameObject.Instantiate(iconCarrier, spawnPoints[knobId].position, Quaternion.identity);
                g.GetComponent<BeamSuckedObject>().suckTarget = suckTarget;
                Material mat = g.GetComponentInChildren<MeshRenderer>().material;

                Texture2D t = icons[spawnID];
                mat.SetTexture("_UnlitColorMap", t);
                mat.SetTexture("_EmissiveColorMap", t);
            }
        }
    }
}
