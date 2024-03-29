﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSteering : MonoBehaviour {
    ShipEntity entity;

    void Awake() {
        entity = GetComponent<ShipEntity>();
    }

    void FixedUpdate() {
        if (!entity.IsOverturned()) {
            if (Input.GetKey(KeyCode.W)) {
                entity.MoveForward();
            }

            if (Input.GetKey(KeyCode.S)) {
                entity.Brake();
            }

            if ((!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) || entity.CurrentWorkingPowerUp?.Type == PowerUpType.Stabilizer) {
                entity.CounterRightLeftRotation();
            }

            if ((!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)) || entity.CurrentWorkingPowerUp?.Type == PowerUpType.Stabilizer) {
                entity.CounterForwardBackRotation();
            }

            if (Input.GetKey(KeyCode.A)) {
                entity.TurnLeft();
            }

            if (Input.GetKey(KeyCode.D)) {
                entity.TurnRight();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                entity.UsePowerUp();
            }
        }
    }
}