using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControinPointLoopSetter : MonoBehaviour {

    [ContextMenu("Set Loop")]
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