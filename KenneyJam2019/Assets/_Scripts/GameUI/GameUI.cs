﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    public Text lapText;
    public Text timeText;
    public Text PowerUpText;
    public Text PowerUpTimeLeft;
    public Text ResetCooldownText;

    public Color NoPowerUpColor;
    public Color PowerUpReadyColor;
    public Color PowerUpActiveColor;

    public Text countdownText;

    public Text playersListText;

    [Header("Pause menu")]
    public GameObject pausePanel;

    RaceManager manager;
    ShipRaceController playerShip;

    StringBuilder stringBuilder;

    [Header("End Game Panel")]
    public GameObject endGamePanel;
    public Text listOfShipsThatFinishedTeRaceText;
    public Text youLostOrYouWinText;

    [Header("Highscores Panel")]
    
    public GameObject highscoresPanel;
    public Text listOfTheBestHighscoresText;

    RaceHighscores raceHighscores;

    List<ShipRaceController> shipsThatFinishedTheRace = new List<ShipRaceController>();
    List<float> raceTimeofShipsThatFinishedTheRace = new List<float>();

    int lapsCount;

    bool paused;

    void Awake() {
        manager = FindObjectOfType<RaceManager>();
        raceHighscores= FindObjectOfType<RaceHighscores>();
        stringBuilder = new StringBuilder();

        ShipRaceController.OnFinishedRace += OnShipFinishedRace;
    }

    void OnDestroy() {
        ShipRaceController.OnFinishedRace -= OnShipFinishedRace;
    }

    private void Start() {
        playerShip = manager.playerShip;
        lapsCount = manager.GetLapsCount();
    }

    void Update() {
        int laps = Mathf.Clamp(playerShip.GetLap(), 0, manager.raceData.lapCount);
        lapText.text = $"Lap: {laps}/{lapsCount}";
        var minutes = (int) (manager.timeSinceRaceStarted / 60);
        var seconds = Mathf.RoundToInt(manager.timeSinceRaceStarted - minutes * 60);

        string minutesStr;
        string secondsStr;
        if(minutes < 10)
        {
            minutesStr = "0" + minutes.ToString();
        }
        else
        {
            minutesStr =  minutes.ToString();
        }

        if(seconds < 10)
        {
            secondsStr = "0" + seconds.ToString();
        }
        else
        {
            secondsStr =  seconds.ToString();
        }
        
        timeText.text = $"Time: {minutesStr}:{secondsStr}";

        if (Input.GetButtonDown("Cancel")) {
            if (paused) {
                paused = false;

                TimeManager.ResumeTime();
                pausePanel.SetActive(false);
            } else {
                paused = true;

                TimeManager.PauseTime();
                pausePanel.SetActive(true);
            }
        }

        if (playerShip.shipEntity.PowerUpReadyToLaunch != null)
        {
            PowerUpText.text = $"{playerShip.shipEntity.PowerUpReadyToLaunch.Name} READY";
            PowerUpText.color = PowerUpReadyColor;
        }
        else if (playerShip.shipEntity.CurrentWorkingPowerUp != null)
        {
            PowerUpText.text = $"{playerShip.shipEntity.CurrentWorkingPowerUp.Name} ACTIVE";
            PowerUpText.color = PowerUpActiveColor;

            PowerUpTimeLeft.text = $"{playerShip.shipEntity.GetPowerUpTimeLeft():0.0} seconds left";
            PowerUpTimeLeft.gameObject.SetActive(true);
        }
        else
        {
            PowerUpText.text = "NO POWERUP";
            PowerUpText.color = NoPowerUpColor;
            PowerUpTimeLeft.color = NoPowerUpColor;
            PowerUpTimeLeft.gameObject.SetActive(false);
        }

        if(manager.raceStarted == false) {
            countdownText.text = string.Format("{0:0.000}", manager.countdownTimer);
        }

        if (playerShip.shipEntity.IsOverturned())
        {
            ResetCooldownText.enabled = true;
            ResetCooldownText.text = $"{playerShip.shipEntity.GetTimeToReset():0.0} TO RESET";
        }
        else
        {
            ResetCooldownText.enabled = false;
        }
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

    public void ResumeGame() {
        TimeManager.ResumeTime();
        pausePanel.SetActive(false);
    }

    public void RestartRace() {
        var currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void ExitToMenu() {
        TimeManager.ResumeTime();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main menu");
    }

    public void GoToHighscores() {
        List<HighscoreEntry> list = raceHighscores.highscores;
        StringBuilder builder = new StringBuilder();
        for(int i = 0; i < list.Count; i++)
        {
            builder.AppendFormat("{0}. {1} - {2:00.00} s\n", i + 1, list[i].name, list[i].time);
        }
        listOfTheBestHighscoresText.text = builder.ToString();
        endGamePanel.SetActive(false);
        highscoresPanel.SetActive(true);
    }

    public void BackFromHighscores() {
        endGamePanel.SetActive(true);
        highscoresPanel.SetActive(false);
    }

    public void OnShipFinishedRace(ShipRaceController ship) {
        shipsThatFinishedTheRace.Add(ship);
        raceTimeofShipsThatFinishedTheRace.Add(manager.timeSinceRaceStarted);

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < shipsThatFinishedTheRace.Count; i++) {
            //@TODO: Add some time stamp
            builder.AppendFormat("{0}. {1} - {2:00.00} s\n", i + 1, shipsThatFinishedTheRace[i].playerData.name, raceTimeofShipsThatFinishedTheRace[i]);
            if (shipsThatFinishedTheRace[0].playerData.steerByAI == false) {
                youLostOrYouWinText.text = "You won";
            }
            else
            {
                youLostOrYouWinText.text = "You lost";
            }
        }
        listOfShipsThatFinishedTeRaceText.text = builder.ToString();

        if (ship.playerData.steerByAI == false) {
            endGamePanel.SetActive(true);
        }
    }
}