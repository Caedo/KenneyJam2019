using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class AngleMultiplier
{
    public int Distance;
    public float Angle;
}

public class ShipAI : MonoBehaviour
{
    public ShipEntity ShipEntity;
    public ShipRaceController RaceController;

    public int CriticalDeltaAngleToBrake;
    public int DistanceBeforeControlPointToBrake;
    public float MinimalSpeedWithBrakingBeforeControlPoint;
    public int AngleTolerance;
    public float EdgeDistanceToChest;
    public float EdgeAngleToChest;
    public float MaxDifferenceBetweenDeltaAngles;
    public List<AngleMultiplier> AngleMultipliers;

    public PowerUpsManager _powerUpsManager;

    void Start()
    {
        _powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }

    void Update()
    {
        // Project meme
        if (Math.Abs(ShipEntity.canMove) < 0.1)
        {
            return;
        }

        var (distanceToNextControlPoint, controlPointDeltaAngle) = GetDistanceAndDeltaAngleToNearestControlPoint();
        var (distanceToNearestChest, nearestChestDeltaAngle) = GetDistanceAndDeltaAngleToNearestChest();

        if (RotateShipTowardChest(distanceToNearestChest, controlPointDeltaAngle, nearestChestDeltaAngle))
        {
            MoveShipForwardOrBrake(distanceToNearestChest, nearestChestDeltaAngle);
        }
        else
        {
            RotateShipTowardControlPoint(distanceToNextControlPoint, controlPointDeltaAngle);
            MoveShipForwardOrBrake(distanceToNextControlPoint, controlPointDeltaAngle);
        }

        UsePowerUpIfCan();
    }

    private (float distance, float deltaAngle) GetDistanceAndDeltaAngleToNearestControlPoint()
    {
        var nextControlPointPosition = RaceController.nextControlPoint.transform.position;
        var distanceToNextControlPoint = Vector3.Distance(nextControlPointPosition, transform.position);
        var vectorToTarget = nextControlPointPosition - transform.position;
        var angle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;
        var deltaAngle = Mathf.DeltaAngle(angle, transform.eulerAngles.y - 180);

        return (distanceToNextControlPoint, deltaAngle);
    }

    private (float distance, float deltaAngle) GetDistanceAndDeltaAngleToNearestChest()
    {
        var nearestChest = _powerUpsManager.GetNearestChest(transform.position);
        if (nearestChest == null)
        {
            return (float.MaxValue, float.MaxValue);
        }

        var distanceToNearestChest = Vector3.Distance(nearestChest.transform.position, transform.position);
        var vectorToChest = nearestChest.transform.position - transform.position;
        var angleToChest = Mathf.Atan2(vectorToChest.x, vectorToChest.z) * Mathf.Rad2Deg;
        var deltaAngleToChest = Mathf.DeltaAngle(angleToChest, transform.eulerAngles.y - 180);

        return (distanceToNearestChest, deltaAngleToChest);
    }

    private void MoveShipForwardOrBrake(float distanceToNextControlPoint, float deltaAngle)
    {
        var currentSpeed = GetComponent<Rigidbody>().velocity.sqrMagnitude;

        if (ShipEntity.IsNearToOverturn() || Math.Abs(deltaAngle) > CriticalDeltaAngleToBrake ||
            (distanceToNextControlPoint < DistanceBeforeControlPointToBrake && currentSpeed > MinimalSpeedWithBrakingBeforeControlPoint))
        {
            ShipEntity.Brake();
        }
        else
        {
            ShipEntity.MoveForward();
        }
    }

    private void RotateShipTowardControlPoint(float distanceToNextControlPoint, float deltaAngle)
    {
        var angleMultiplier = AngleMultipliers.First(p => distanceToNextControlPoint >= p.Distance).Angle;

        if (!ShipEntity.IsNearToOverturn())
        {
            if (deltaAngle < -(AngleTolerance * angleMultiplier))
            {
                ShipEntity.TurnRight();
            }
            else if (deltaAngle > AngleTolerance * angleMultiplier)
            {
                ShipEntity.TurnLeft();
            }
        }

        ShipEntity.CounterRightLeftRotation();
        ShipEntity.CounterForwardBackRotation();
    }

    private bool RotateShipTowardChest(float distanceToNextControlPoint, float controlPointDeltaAngle, float nearestChestDeltaAngle)
    {
        if (Math.Abs(controlPointDeltaAngle - nearestChestDeltaAngle) > MaxDifferenceBetweenDeltaAngles)
        {
            return false;
        }

        if (distanceToNextControlPoint < EdgeDistanceToChest)
        {
            if (Math.Abs(nearestChestDeltaAngle) < EdgeAngleToChest)
            {
                if (nearestChestDeltaAngle < 0)
                {
                    ShipEntity.TurnRight();
                }
                else if (nearestChestDeltaAngle > 0)
                {
                    ShipEntity.TurnLeft();
                }

                return true;
            }
        }

        return false;
    }

    private void UsePowerUpIfCan()
    {
        if (ShipEntity.PowerUpReadyToLaunch != null)
        {
            switch (ShipEntity.PowerUpReadyToLaunch.Type)
            {
                case PowerUpType.Acceleration:
                {
                    ShipEntity.UsePowerUp();
                    break;
                }
            }
        }
    }
}
