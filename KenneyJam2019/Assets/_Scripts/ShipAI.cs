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
        var currentSpeed = GetComponent<Rigidbody>().velocity.sqrMagnitude;

        var deltaAngle = Mathf.DeltaAngle(angle, transform.eulerAngles.y - 180);
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

        Debug.Log($"Angle: {deltaAngle}, Speed: {currentSpeed}, Dist: {distanceToNextControlPoint}, Multi: {angleMultiplier}");

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
}
