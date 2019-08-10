using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public Text lapText;
    public Text timeText;

    public Text playersListText;

    RaceManager manager;
    ShipRaceController playerShip;

    StringBuilder stringBuilder;

    int lapsCount;

    void Awake() {
        manager = FindObjectOfType<RaceManager>();
        stringBuilder = new StringBuilder();
    }

    private void Start() {
        playerShip = manager.playerShip;
        lapsCount = manager.GetLapsCount();
    }

    void Update() {
        lapText.text = $"Lap: {playerShip.GetLap()}/{lapsCount}";
        var minutes = (int) (manager.timeSinceRaceStarted / 60);
        var seconds = Mathf.RoundToInt(manager.timeSinceRaceStarted - minutes * 60);
        timeText.text = $"Time: {minutes}:{seconds}";
    }

    void LateUpdate() {
        var list = manager.GetShipsList();
        stringBuilder.Clear();

        for (int i = 0; i < list.Count; i++) {
            string name = list[i].playerData.name;
            stringBuilder.AppendFormat("{0}. {1}\n", i + 1, name);
        }

        playersListText.text = stringBuilder.ToString();
    }
}