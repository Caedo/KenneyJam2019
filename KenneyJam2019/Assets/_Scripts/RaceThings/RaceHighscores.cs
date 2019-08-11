using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HighscoreEntry
{
    public HighscoreEntry(float t, string n)
    {
        time = t;
        name = n;
    }
    public float time;
    public string name;
}

public class RaceHighscores : MonoBehaviour
{
    
    public RaceManager raceManager;

    public List<HighscoreEntry> highscores { get; private set; }

    void Awake()
    {
        ShipRaceController.OnFinishedRace += OnFinishedRace;
        highscores = new List<HighscoreEntry>();
        for(int i = 0; i < 5; i++)
        {
            if(PlayerPrefs.HasKey("Highscore_time_" + i.ToString()))
            {
                float score = PlayerPrefs.GetFloat("Highscore_time_" + i.ToString());
                string name = PlayerPrefs.GetString("Highscore_name_" + i.ToString());
                highscores.Add(new HighscoreEntry(score, name));
            }
        }
    }

    void Update()
    {
        
    }

    void OnFinishedRace(ShipRaceController shipRaceController)
    {
        if(shipRaceController.playerData.steerByAI == false)
        {
            // We have player
            highscores.Add(new HighscoreEntry(raceManager.timeSinceRaceStarted, shipRaceController.playerData.name));
            highscores = highscores.OrderBy(q => q.time).ToList();

            // Cut to the best five
            while(highscores.Count > 5)
            {
                highscores.RemoveAt(highscores.Count - 1);
            }

            // Save highscores
            for(int i = 0; i < 5; i++)
            {
                if(highscores.Count > i)
                {
                    PlayerPrefs.SetFloat("Highscore_time_" + i.ToString(), highscores[i].time);
                    PlayerPrefs.SetString("Highscore_name_" + i.ToString(), highscores[i].name);
                }
                else
                {
                    if(PlayerPrefs.HasKey("Highscore_time_" + i.ToString()))
                    {
                        PlayerPrefs.DeleteKey("Highscore_time_" + i.ToString());
                        PlayerPrefs.DeleteKey("Highscore_name_" + i.ToString());
                    }
                }
            }
            PlayerPrefs.Save();
        }
    }

    void ResetHighscores()
    {
        highscores.Clear();
    }
}
