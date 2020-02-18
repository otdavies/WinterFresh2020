using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingDriver : Observer<int[]>
{
    public GameObject observerable;
    public int knobId;
    public float A;
    public float B;
    public float blendTime = 10;
    private float _targetValue;
    private VolumeProfile _profile;
    //private WhiteBalance _whiteBalance;

    // private void Start()
    // {
    //     RegisterWith(observerable.GetComponent<Observable<int[]>>());
    //     _profile = GetComponent<Volume>().sharedProfile;
    //     _profile.TryGetSettings(out _whiteBalance);
    // }

    // private void Update()
    // {
    //     _profile.temperature.value = Color.Lerp(_profile.temperature.value, _targetValue, Time.deltaTime * blendTime);
    // }

    public override void Observed(ref int[] val)
    {
        _targetValue = Mathf.Lerp(A, B, val[knobId] / 64f);
    }
}
