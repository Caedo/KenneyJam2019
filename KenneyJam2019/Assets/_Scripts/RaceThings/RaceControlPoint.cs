using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceControlPoint : MonoBehaviour {
    public bool isEndPoint;
    public RaceControlPoint nextPoint;

    void OnDrawGizmos() {
        Gizmos.color = Color.green;

        if (nextPoint != null)
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
    }
}