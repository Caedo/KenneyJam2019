using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public string name;
    public bool steerByAI;
}

[CreateAssetMenu]
public class RaceData : ScriptableObject {
    public int lapCount;

    public List<PlayerData> players;

    public int PlayersCount => players.Count;
}