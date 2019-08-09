using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRaceController : MonoBehaviour {
    RaceControlPoint nextControllPoint;

    int crossedControlPointCount;

    bool canMove;
    bool raceStarted;

    float raceTime;

    private void Start() {
        canMove = false;
        raceStarted = false;
    }

    void Update() {
        if (raceStarted) {
            raceTime += Time.deltaTime;
        }
    }

    public void RaceStart() {
        canMove = true;
        raceStarted = true;
    }

    public void CrossedControlPoint(RaceControlPoint point) {
        crossedControlPointCount++;

        nextControllPoint = point.nextPoint;
    }
}