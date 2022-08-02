using UnityEngine;

public class TrajectoryRenderer : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void ShowTrajectory(Vector3 origin, Vector3 speed, float mass)
    {
        var points = new Vector3[100];
        _lineRenderer.positionCount = points.Length;
        var quaternion = Quaternion.FromToRotation(speed.normalized, speed.normalized + -Vector3.up * speed.normalized.y);
        var angle = Quaternion.Angle(Quaternion.identity, quaternion);

        var fromTo = speed - origin;
        var fromToXZ = new Vector3(fromTo.x, 0, fromTo.z);

        var y = fromToXZ.magnitude;
        var x = fromTo.y;

        float angleInRadius = angle * Mathf.PI / 180;
        
        for (int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + time * speed + Physics.gravity * time * time / 2;

            if (points[i].y < origin.y)
            {
                _lineRenderer.positionCount = i;
                break;
            }
        }
        
        _lineRenderer.SetPositions(points);
    }

    public void ClearTrajectoty()
    {
        _lineRenderer.positionCount = 0;
    }
}
