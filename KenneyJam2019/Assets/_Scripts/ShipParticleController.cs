using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParticleController : MonoBehaviour {
    public ParticleSystem dustParicles;

    ShipEntity entity;

    void Awake() {
        entity = GetComponent<ShipEntity>();
    }

    private void Update() {
        if (entity.IsGounded) {
            dustParicles.Play();
        } else {
            dustParicles.Stop();
        }
    }

}