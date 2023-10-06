using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MapGizmo : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] protected float _resolution = 50;

    public void Draw(Vector2[] points)
    {
        var pointsWithResolution = new List<Vector3>();
        for (int i = 0; i < points.Length; i++)
        {
            if (i == 0 || i == points.Length - 1 || i % (int)(_resolution / Time.timeScale) == 0)
            {
                pointsWithResolution.Add(points[i].ToVector3());
            }
        }
        _lineRenderer.positionCount = pointsWithResolution.Count;
        _lineRenderer.SetPositions(pointsWithResolution.ToArray());
    }
}