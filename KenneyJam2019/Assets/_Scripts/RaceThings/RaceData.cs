using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public string name;
    public Color color;
    public bool steerByAI;
}

[CreateAssetMenu]
public class RaceData : ScriptableObject {
    public int lapCount;

    public List<PlayerData> players;

    public int PlayersCount => players.Count;

    static readonly string[] names = { "Jack Spearow", "Var The Poor", "Kappa Warrior", "Nick The Temporary", "Marian von CanMove" };
    static readonly Color[] colors = { Color.red, Color.magenta, Color.blue, Color.cyan, Color.green };

    public void CreateData(int npcNumber, PlayerData playerData) {
        players.Clear();
        players.Add(playerData);

        List<string> localNames = new List<string>(names);
        List<Color> localColors = new List<Color>(colors);

        for (int i = 0; i < npcNumber; i++) {
            int index = Random.Range(0, localNames.Count);

            var randomName = localNames[index];
            var randomColor = localColors[index];

            localNames.RemoveAt(index);

            if(randomColor == playerData.color) {
                index = (index + 1) % localColors.Count;
                randomColor = localColors[index];
            }
            localColors.RemoveAt(index);

            players.Add(new PlayerData() {
                name = randomName,
                color = randomColor,
                    steerByAI = true,
            });
        }
    }

}