using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using UnityEngine;

public class RaceManager : MonoBehaviour {
    public RaceData raceData;
    public ShipRaceController playerShipPrefab;
    public ShipRaceController AIShipPrefab;
    public ControlPointArrowEntity ArrowEntity;

    [Header("Start Race Things")]
    public StartLine startLine;
    public RaceControlPoint firstControlPoint;
    public float startCountdownTime;

    [Header("Camera")]
    new public CameraEntity camera;

    [Header("Events")]
    public GameEvent OnRaceStart;

    List<ShipRaceController> ships = new List<ShipRaceController>();

    List<ShipRaceController> shipsThatFinishedTheRace = new List<ShipRaceController>();

    [HideInInspector]
    public float countdownTimer;

    public ShipRaceController playerShip { get; private set; }
    public float timeSinceRaceStarted { get; private set; }

    [HideInInspector]
    public bool raceStarted;

    void Awake() {
        int playersCount = raceData.PlayersCount;

        var positions = startLine.GetStartPositionsForPlayers(playersCount);
        for (int i = 0; i < playersCount; i++) {
            var playerData = raceData.players[i];

            var prefab = playerData.steerByAI ? AIShipPrefab : playerShipPrefab;
            var ship = Instantiate(prefab, positions[i], startLine.transform.rotation);
            ship.raceData = raceData;
            ship.nextControlPoint = firstControlPoint;
            ship.playerData = playerData;

            ship.SetColor(playerData.color);

            ship.name = raceData.players[i].name + "Ship";

            if (raceData.players[i].steerByAI == false) {
                camera.CameraTarget = ship.transform.Find("CameraTarget").gameObject;
                ArrowEntity.Entity = ship.shipEntity;
                ArrowEntity.RaceManager = ship;
                playerShip = ship;
            }

            ships.Add(ship);
        }

        ShipRaceController.OnFinishedRace += OnShipFinishRace;
    }

    void Start() {
        StartRace();
    }

    void StartRace() {
        raceStarted = false; // that's not bug...
        StartCoroutine(CountingRoutine());
    }

    void Update() {
        ships.Sort((a, b) => {
            (int aLaps, float aDist) = a.GetRaceDistanceTuple();
            (int bLaps, float bDist) = b.GetRaceDistanceTuple();

            if (aLaps == bLaps) {
                return (aDist < bDist) ? -1 : 1;
            } else {
                return (aLaps < bLaps) ? 1 : -1;
            }
        });

        if (raceStarted) {
            timeSinceRaceStarted += Time.deltaTime;
        }
    }

    public int GetLapsCount() {
        return raceData.lapCount;
    }

    IEnumerator CountingRoutine() {
        countdownTimer = startCountdownTime;

        while (countdownTimer > 0) {
            yield return null;

            countdownTimer -= Time.deltaTime;
        }
        Debug.Log("START!!!");
        raceStarted = true;
        OnRaceStart.Raise();
    }

    void OnShipFinishRace(ShipRaceController ship) {
        shipsThatFinishedTheRace.Add(ship);
    }

    public List<ShipRaceController> GetShipsList() {
        return ships;
    }
}