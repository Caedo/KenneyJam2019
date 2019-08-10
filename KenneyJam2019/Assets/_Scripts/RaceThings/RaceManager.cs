using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using UnityEngine;

public class RaceManager : MonoBehaviour {
    public RaceData raceData;
    public ShipRaceController shipPrefab;

    [Header("Start Race Things")]
    public StartLine startLine;
    public RaceControlPoint firstControlPoint;
    public float startCountdownTime;

    [Header("Camera")]
    new public CameraEntity camera;

    [Header("Events")]
    public GameEvent OnRaceStart;

    List<ShipRaceController> ships = new List<ShipRaceController>();

    float countdownTimer;
    bool counting;

    void Awake() {
        int playersCount = raceData.PlayersCount;

        var positions = startLine.GetStartPositionsForPlayers(playersCount);
        for (int i = 0; i < playersCount; i++) {
            var ship = Instantiate(shipPrefab, positions[i], startLine.transform.rotation);
            ship.raceData = raceData;
            ship.nextControlPoint = firstControlPoint;

            ship.name = raceData.players[i].name + "Ship";

            if (raceData.players[i].steerByAI == false) camera.CameraTarget = ship.transform.Find("CameraTarget").gameObject;

            ships.Add(ship);
        }

        ShipRaceController.OnFinishedRace += OnShipFinishRace;
    }

    void Start() {
        StartRace();
    }

    void StartRace() {
        StartCoroutine(CountingRoutine());
    }

    void Update() {
        ships.Sort((a, b) => {
            (int aLaps, float aDist) = a.GetRaceDistanceTuble();
            (int bLaps, float bDist) = b.GetRaceDistanceTuble();

            if (aLaps == bLaps) {
                return (aDist < bDist) ? -1 : 1;
            } else {
                return (aLaps < bLaps) ? -1 : 1;
            }

        });
    }

    IEnumerator CountingRoutine() {
        countdownTimer = startCountdownTime;
        counting = true;

        while (countdownTimer > 0) {
            yield return null;

            countdownTimer -= Time.deltaTime;
        }
        Debug.Log("START!!!");
        OnRaceStart.Raise();
    }

    void OnShipFinishRace(ShipRaceController ship) {

    }
}