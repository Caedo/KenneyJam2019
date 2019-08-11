using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControinPointLoopSetter : MonoBehaviour {

    public Material lineMaterial;
    public Color lineColor;
    public float lineWidth;

    [ContextMenu("Set Line")]
    public void SetLineRenderer() {
        var line = GetComponent<LineRenderer>();
        if (!line) {
            line = gameObject.AddComponent<LineRenderer>();
        }

        line.useWorldSpace = true;
        line.loop = true;
        line.positionCount = transform.childCount;
        line.material = lineMaterial;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;

        var points = new Vector3[transform.childCount];
        for (int i = 0; i < points.Length; i++) {
            points[i] = transform.GetChild(i).position + Vector3.up * 5f;
        }

        line.SetPositions(points);
    }

    public void SetLoop() {
        // Assuming that children are in order...
        for (int i = 0; i < transform.childCount - 1; i++) {
            var pointA = transform.GetChild(i).GetComponent<RaceControlPoint>();
            var pointB = transform.GetChild(i + 1).GetComponent<RaceControlPoint>();

            pointA.nextPoint = pointB;
        }

        var lastPoint = transform.GetChild(transform.childCount - 1).GetComponent<RaceControlPoint>();
        var firsPoint = transform.GetChild(0).GetComponent<RaceControlPoint>();

        lastPoint.nextPoint = firsPoint;
    }
}