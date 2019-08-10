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

    static readonly string[] names = { "Jack Spearow", "Var The Poor", "Kappa Warrior", "Nick The Temporary" };

    public void CreateData(int npcNumber, PlayerData playerData) {
        players.Clear();
        players.Add(playerData);

        List<string> localNames = new List<string>(names);

        for (int i = 0; i < npcNumber; i++) {
            int nameIndex = Random.Range(0, localNames.Count);
            var randomName = localNames[nameIndex];
            localNames.RemoveAt(nameIndex);

            players.Add(new PlayerData() {
                name = randomName,
                    steerByAI = true,
            });
        }
    }

}