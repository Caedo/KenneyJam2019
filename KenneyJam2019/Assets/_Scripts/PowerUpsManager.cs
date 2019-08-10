using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public List<ChestEntity> Chests;
    public List<PowerUpData> PowerUps;

    private System.Random _random;

    void Start()
    {
        _random = new System.Random();
    }

    void Update()
    {
        
    }

    public ChestEntity GetNearestChest(Vector3 position)
    {
        return Chests
            .Where(p => p.IsActive)
            .Select(p => new {Chest = p, Dist = Vector3.Distance(position, p.transform.position)})
            .OrderBy(p => p.Dist)
            .FirstOrDefault()
            ?.Chest;
    }

    public PowerUpData GetRandom()
    {
        return PowerUps[_random.Next(PowerUps.Count)];
    }
}
