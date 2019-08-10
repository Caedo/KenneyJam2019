using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceHighscores : MonoBehaviour
{
    public RaceManager raceManager;

    public List<float> highscores { get; private set; }

    void Awake()
    {
        ShipRaceController.OnFinishedRace += OnFinishedRace;
        highscores = new List<float>();
    }

    void Update()
    {
        
    }

    void OnFinishedRace(ShipRaceController shipRaceController)
    {
        if(shipRaceController.playerData.steerByAI == false)
        {
            // We have player
            highscores.Add(raceManager.timeSinceRaceStarted);
            highscores.Sort();
        }
    }

    void ResetHighscores()
    {
        highscores.Clear();
    }
}
