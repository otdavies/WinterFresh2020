using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamSuckedObject : MonoBehaviour
{
    public Transform suckTarget;
    public float suckTime = 3;
    public float suckTargetStrength;
    public float scaleBulgeStrength = 0.3f;
    private BezierPath _path;
    private MeshRenderer _renderer;
    private Vector3[] _controlPoints; 
    private Vector3 _originalScale;

    private void Start()
    {
        suckTime += Random.Range(0f, 1f);
        _controlPoints = new Vector3[4];
        _controlPoints[0] = transform.position;
        _controlPoints[1] = transform.position + transform.up;
        _controlPoints[2] = suckTarget.position + suckTarget.up;
        _controlPoints[3] = suckTarget.position;

        _originalScale = transform.localScale;

        _path = new BezierPath(_controlPoints);
        GetSuckedUp();
    }

    public void GetSuckedUp()
    {
        StartCoroutine(Succ());
    }

    public void Respawn(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        transform.up = direction;
        GetSuckedUp();
    }

    private IEnumerator Succ()
    {
        float step = Time.deltaTime / suckTime;
        float t = 0;
        while(t <= 1)
        {
            t += step;
            float tt = t*t*t * (t * (6f*t - 15f) + 10f);
            _controlPoints[2] = suckTarget.position + suckTarget.up * suckTargetStrength;
            _controlPoints[3] = suckTarget.position;
            _path.ControlPoints = _controlPoints;

            Vector3 pos, dir;
            _path.GetOrientationOnPath(tt, out pos, out dir);

            transform.position = pos;
            transform.up = dir;
            transform.localScale = _originalScale + Lerp3(Vector3.zero, Vector3.one, Vector3.zero, (tt * 2 - 1)) * scaleBulgeStrength;

            yield return new WaitForEndOfFrame();
        }
        Respawn(_controlPoints[1], _controlPoints[1] - _controlPoints[0]);
    }

    private Vector3 Lerp3(Vector3 A, Vector3 B, Vector3 C, float t)
    {
        return Vector3.Lerp( Vector3.Lerp(A, B, 1 + Mathf.Clamp(t, -1, 0)), C, Mathf.Clamp(t, 0, 1) );
    }
}
