using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUpType
{
    None,
    Acceleration
}

[CreateAssetMenu]
public class PowerUpData : ScriptableObject
{
    public PowerUpType Type;
    public string Name;
    public int Time;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}