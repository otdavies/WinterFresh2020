using UnityEngine;

public class BezierPath
{
    public Vector3[] ControlPoints { get => p; set => p = value; }
    private Vector3[] p;

    public BezierPath(Vector3[] points)
    {
        ControlPoints = points;
    }

    public void GetOrientationOnPath(float t, out Vector3 position, out Vector3 direction)
    {
        t = Mathf.Clamp01(t);
        float tt = t*t;
        float it = (1 - t);
        float itt = it*it;
        position = (it*itt) * p[0] + (3*itt*t) * p[1] + (3*it*tt) * p[2] + (t*tt) * p[3];
        direction = (3*itt) * (p[1] - p[0]) + (6*it*t) * (p[2] - p[1]) + (3*tt) * (p[3] - p[2]);
    }

    public void DrawDebug()
    {
        Debug.DrawLine(ControlPoints[0], ControlPoints[1], Color.red);
        Debug.DrawLine(ControlPoints[2], ControlPoints[3], Color.red);
        Debug.DrawLine(ControlPoints[0], ControlPoints[3], Color.white);
        for (var i = 1; i <= 16; i++)
        {
            Vector3 pos1, dir1;
            Vector3 pos2, dir2;
            GetOrientationOnPath((i-1)/16f, out pos1, out dir1);
            GetOrientationOnPath(i/16f, out pos2, out dir2);
            Debug.DrawLine(pos1, pos2, Color.green);
        }
    }
}
