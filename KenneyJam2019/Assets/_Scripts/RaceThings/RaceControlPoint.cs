using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceControlPoint : MonoBehaviour {
    public bool isEndPoint;
    public RaceControlPoint nextPoint;

    void OnTriggerEnter(Collider other) {
        var ship = other.GetComponent<ShipRaceController>();
        if (ship) {
            ship.CrossedControlPoint(this);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;

        if (nextPoint != null)
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
    }
}