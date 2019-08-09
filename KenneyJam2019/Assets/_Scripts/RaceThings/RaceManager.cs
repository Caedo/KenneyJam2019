using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour {
    public RaceData raceData;

    public StartLine startLine;
    public GameObject shipPrefab;

    void Awake() {
        int playersCount = raceData.PlayersCount;

        var positions = startLine.GetStartPositionsForPlayers(playersCount);
        for (int i = 0; i < playersCount; i++)
        {
            var player = Instantiate(shipPrefab, positions[i], startLine.transform.rotation);
        }
    }
}