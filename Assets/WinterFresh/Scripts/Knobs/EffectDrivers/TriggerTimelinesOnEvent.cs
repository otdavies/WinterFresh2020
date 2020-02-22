using UnityEngine;
using UnityEngine.Playables;
 
public class TriggerTimelinesOnEvent : Observer<NetworkIntMessage>
{
    public PlayableAsset begin;
    public PlayableAsset end;
    private PlayableDirector player;

    [Header("Effect Color controls")]
    public Color[] colors;
    public Texture[] textures;
    public Material[] materials;
 
    // Use this for initialization
    private void Start()
    {
        player = GetComponent<PlayableDirector>();
        player.playOnAwake = false;
    }

    public override void Observed(ref NetworkIntMessage msg)
    {
        if(msg.MsgType == NetworkIntMessage.MessageType.STATE)
        {
            bool activate = msg.data[1] == 1 ? true : false;
            if(activate) 
            {
                //player.Stop();
                player.playableAsset = begin;
                player.Play();
            }
            else
            {
                //player.Stop();
                player.playableAsset = end;
                player.Play();
            }
        }

        if(msg.MsgType == NetworkIntMessage.MessageType.COLOR)
        {
            Color c = colors[msg.data[1]];
            Texture t = textures[msg.data[1]];
            for(int i = 0; i < materials.Length; i++)
            {
                materials[i].SetTexture("EdgeTex", t);
                materials[i].SetColor("EffectColor", c);
            }
        }
    }
}