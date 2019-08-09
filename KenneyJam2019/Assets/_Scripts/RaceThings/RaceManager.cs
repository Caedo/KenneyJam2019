using System.Collections;
using System.Collections.Generic;
using RoboRyanTron.Events;
using UnityEngine;

public class RaceManager : MonoBehaviour {
    public RaceData raceData;
    public GameObject shipPrefab;

    [Header("Start Race Things")]
    public StartLine startLine;
    public float startCountdownTime;

    [Header("Events")]
    public GameEvent OnRaceStart;

    float countdownTimer;
    bool counting;

    void Awake() {
        int playersCount = raceData.PlayersCount;

        var positions = startLine.GetStartPositionsForPlayers(playersCount);
        for (int i = 0; i < playersCount; i++) {
            var player = Instantiate(shipPrefab, positions[i], startLine.transform.rotation);
        }
    }

    void StartRace() {
        StartCoroutine(CountingRoutine());
    }

    IEnumerator CountingRoutine() {
        countdownTimer = startCountdownTime;
        counting = true;

        while (countdownTimer > 0) {
            yield return null;

            countdownTimer -= Time.deltaTime;
        }

        OnRaceStart.Raise();
    }
}