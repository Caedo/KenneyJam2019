using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRaceController : MonoBehaviour {

    public static System.Action<ShipRaceController> OnFinishedRace;
    public RaceData raceData;

    public RaceControlPoint nextControlPoint;

    ShipEntity shipEntity;

    int crossedControlPointCount;

    bool canMove;
    bool raceStarted;

    int lapNumber;

    void Awake() {
        shipEntity = GetComponent<ShipEntity>();
    }

    private void Start() {
        canMove = false;
        shipEntity.canMove = 0;
    }

    public void RaceStart() {
        canMove = true;
        shipEntity.canMove = 1;
    }

    void OnTriggerEnter(Collider other) {
        var controlPoint = other.GetComponentInParent<RaceControlPoint>();
        if (controlPoint && controlPoint == nextControlPoint) {
            nextControlPoint = controlPoint.nextPoint;
            crossedControlPointCount++;

            if (controlPoint.isEndPoint) {
                lapNumber++;

                if (raceData != null && lapNumber > raceData.lapCount) {
                    FinishRace();
                }
            }
        }
    }

    void FinishRace() {
        OnFinishedRace?.Invoke(this);

        shipEntity.canMove = 0;
    }

    public void CrossedControlPoint(RaceControlPoint point) {
        crossedControlPointCount++;

        nextControlPoint = point.nextPoint;
    }

    public(int, float) GetRaceDistanceTuble() {
        var srqDist = Vector3.SqrMagnitude(transform.position - nextControlPoint.transform.position);

        return (lapNumber, srqDist);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if (nextControlPoint)
            Gizmos.DrawLine(transform.position, nextControlPoint.transform.position);
    }
}