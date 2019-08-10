using System;
using UnityEngine;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

public class ShipEntity : MonoBehaviour
{
    public float ForwardForce;
    public float BackwardForce;
    public float RotationForce;
    public float InclinationForce;
    public float AntiSinkRightLeftForce;
    public float AntiSinkForwardBackForce;
    public bool IsPlayer;
    public int SecondsBeforeReset;
    public float MaxSpeedToReset;
    public int CriticalAngleToOverturn;
    public int CriticalAngleToNearOverturn;

    public PowerUpData PowerUpReadyToLaunch;
    public PowerUpData CurrentWorkingPowerUp;

    private Rigidbody _rigidbody;
    private bool _collisionDetected;
    private DateTime? _overturnedTimeStart;
    private DateTime? _powerUpTimeStart;
    private float _bonusForwardForce;
    private PowerUpsManager _powerUpsManager;

    [HideInInspector]
    public float canMove = 1; // 0 as not, 1 as sail to the end of th world!

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }

    void FixedUpdate()
    {
        IsOverturned();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _collisionDetected = true;
        }
        else if (collision.gameObject.CompareTag("Chest"))
        {
            PowerUpReadyToLaunch = _powerUpsManager.GetRandom();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _collisionDetected = false;
        }
    }

    public void MoveForward()
    {
        if (_collisionDetected)
        {
            if (!IsShipCriticalAnglePassed())
            {
                _rigidbody.AddRelativeForce(Vector3.forward * -(ForwardForce + _bonusForwardForce) * canMove);
            }
            else
            {
                Debug.Log("Can't move ship, overturned");
            }
        }
    }

    public void Brake()
    {
        if (_collisionDetected)
        {
            if (_rigidbody.velocity.z < 0)
            {
                _rigidbody.AddRelativeForce(Vector3.forward * BackwardForce * canMove);
            }
        }
    }

    public void TurnRight()
    {
        if (!IsShipCriticalAnglePassed())
        {
            _rigidbody.AddRelativeTorque(0, RotationForce * canMove, 0);
            _rigidbody.AddRelativeTorque(0, 0, -InclinationForce * canMove);
        }
    }

    public void TurnLeft()
    {
        if (!IsShipCriticalAnglePassed())
        {
            _rigidbody.AddRelativeTorque(0, -RotationForce * canMove, 0);
            _rigidbody.AddRelativeTorque(0, 0, InclinationForce * canMove);
        }
    }

    public void CounterRightLeftRotation()
    {
        if (transform.localEulerAngles.z > 10 && transform.localEulerAngles.z < 350)
        {
            var add = transform.localEulerAngles.z < 180;
            _rigidbody.AddRelativeTorque(0, 0, AntiSinkRightLeftForce * canMove * (add ? -1 : 1));
        }
    }

    public void CounterForwardBackRotation()
    {
        if (transform.localEulerAngles.x > 10 && transform.localEulerAngles.x < 350)
        {
            var add = transform.localEulerAngles.x < 180;
            _rigidbody.AddRelativeTorque(AntiSinkForwardBackForce * canMove * (add ? -1 : 1), 0, 0);
        }
    }

    public bool IsOverturned()
    {
        if (_collisionDetected && _rigidbody.velocity.sqrMagnitude < MaxSpeedToReset && IsShipCriticalAnglePassed())
        {
            if (_overturnedTimeStart == null)
            {
                _overturnedTimeStart = DateTime.Now;
                Debug.Log("Ship overturned, waiting for reset...");
            }

            if ((DateTime.Now - _overturnedTimeStart).Value.TotalSeconds >= SecondsBeforeReset)
            {
                ResetShipPosition();
                Debug.Log("Ship position reset");
            }

            return true;
        }

        return false;
    }

    public void ResetShipPosition()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _overturnedTimeStart = null;
    }

    private bool IsShipCriticalAnglePassed()
    {
        return transform.localEulerAngles.z > CriticalAngleToOverturn &&
               transform.localEulerAngles.z < 360 - CriticalAngleToOverturn;
    }

    public bool IsNearToOverturn()
    {
        return transform.localEulerAngles.z > CriticalAngleToNearOverturn &&
               transform.localEulerAngles.z < 360 - CriticalAngleToNearOverturn;
    }

    public void UsePowerUp()
    {
        if (PowerUpReadyToLaunch == null || CurrentWorkingPowerUp != null)
        {
            return;
        }

        CurrentWorkingPowerUp = PowerUpReadyToLaunch;
        PowerUpReadyToLaunch = null;

        switch (CurrentWorkingPowerUp.Type)
        {
            case PowerUpType.Acceleration:
            {
                _bonusForwardForce = 10000;
                break;
            }
        }

        _powerUpTimeStart = DateTime.Now;
    }

    private void StopPowerUp()
    {
        _bonusForwardForce = 0;
    }
}
