using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParticleController : MonoBehaviour {
    public ParticleSystem dustParicles;
    public ParticleSystem windParticles;
    public ParticleSystem rotationParticles;
    public ParticleSystem stabilityParticles;

    ShipEntity entity;

    void Awake() {
        entity = GetComponent<ShipEntity>();

        entity.OnPowerUpEnded += PowerUpEnded;
        entity.OnPowerUpUsed += PowerUpUsed;
    }

    private void Update() {
        if (entity.IsGounded) {
            dustParicles.Play();
        } else {
            dustParicles.Stop();
        }
    }

    void PowerUpUsed(PowerUpData powerUp) {
        switch (powerUp.Type) {
            case PowerUpType.Acceleration:
                windParticles.Play();
                break;
            case PowerUpType.Rotation:
                rotationParticles.Play();
                break;
            case PowerUpType.Stabilizer:
                stabilityParticles.Play();
                break;
        }
    }

    void PowerUpEnded(PowerUpData powerUp) {
        switch (powerUp.Type) {
            case PowerUpType.Acceleration:
                windParticles.Stop();
                break;
            case PowerUpType.Rotation:
                rotationParticles.Stop();
                break;
            case PowerUpType.Stabilizer:
                stabilityParticles.Stop();
                break;
        }
    }

}