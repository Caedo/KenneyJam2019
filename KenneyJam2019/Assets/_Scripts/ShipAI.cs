using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private PowerUpsManager _powerUpsManager;

    private readonly Dictionary<int, float> _angleMultipliers = new Dictionary<int, float>
    {
        { int.MaxValue, 1f },
        { 500, 2f },
        { 100, 5f },
        { 0, 10f }
    };

    void Start()
    {
        _powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }

    void FixedUpdate()
    {
        // Project meme
        if (Math.Abs(ShipEntity.canMove) < 0.1)
        {
            return;
        }

        var nextControlPointPosition = RaceController.nextControlPoint.transform.position;
        var distanceToNextControlPoint = Vector3.Distance(nextControlPointPosition, transform.position);
        var vectorToTarget = nextControlPointPosition - transform.position;
        var angle = Mathf.Atan2(vectorToTarget.x, vectorToTarget.z) * Mathf.Rad2Deg;

        var deltaAngle = Mathf.DeltaAngle(angle, transform.eulerAngles.y - 180);

        MoveShipForwardOrBrake(distanceToNextControlPoint, deltaAngle);
        if (!RotateShipTowardChest())
        {
            RotateShipTowardControlPoint(distanceToNextControlPoint, deltaAngle);
        }

        UsePowerUpIfCan();
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
        var angleMultiplier = _angleMultipliers.ToList().First(p => p.Key <= distanceToNextControlPoint).Value;

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

    private bool RotateShipTowardChest()
    {
        var nearestChest = _powerUpsManager.GetNearestChest(transform.position);
        var distanceToNearestChest = Vector3.Distance(nearestChest.transform.position, transform.position);

        if (distanceToNearestChest < EdgeDistanceToChest)
        {
            var vectorToChest = nearestChest.transform.position - transform.position;
            var angleToChest = Mathf.Atan2(vectorToChest.x, vectorToChest.z) * Mathf.Rad2Deg;
            var deltaAngleToChest = Mathf.DeltaAngle(angleToChest, transform.eulerAngles.y - 180);

            if (Math.Abs(deltaAngleToChest) < EdgeAngleToChest)
            {
                if (deltaAngleToChest < 0)
                {
                    ShipEntity.TurnRight();
                }
                else if (deltaAngleToChest > 0)
                {
                    ShipEntity.TurnLeft();
                }

                Debug.Log(deltaAngleToChest);
                return true;
            }
        }

        return false;
    }

    private void UsePowerUpIfCan()
    {
        if(ShipEntity.PowerUpReadyToLaunch != null)
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
