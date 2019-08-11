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

    public float ForwardForceBonus;
    public float AntiSinkForceBonus;
    public float RotationForceBonus;

    public AudioClip ChestCollisionSound;
    public AudioClip PowerUpEnabledSound;
    public AudioClip PowerUpDisabledSound;
    public AudioClip OverturnSound;

    private Rigidbody _rigidbody;
    private bool _collisionDetected;
    private DateTime? _overturnedTimeStart;
    private DateTime? _powerUpTimeStart;
    private float _bonusForwardForce;
    private float _bonusAntiSinkForce;
    private float _bonusRotationForce;
    private PowerUpsManager _powerUpsManager;

    [HideInInspector]
    public float canMove = 1; // 0 as not, 1 as sail to the end of th world!

    public bool IsGounded => _collisionDetected;

    public event System.Action<PowerUpData> OnPowerUpUsed;
    public event System.Action<PowerUpData> OnPowerUpEnded;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }

    void FixedUpdate()
    {
        if (CurrentWorkingPowerUp != null && GetPowerUpTimeLeft() <= 0)
        {
            OnPowerUpEnded?.Invoke(CurrentWorkingPowerUp);
            StopPowerUp();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _collisionDetected = true;
        }
        else if (collision.gameObject.CompareTag("Chest"))
        {
            if (CurrentWorkingPowerUp != null)
            {
                _powerUpTimeStart = DateTime.Now;
            }
            else if (PowerUpReadyToLaunch == null)
            {
                PowerUpReadyToLaunch = _powerUpsManager.GetRandom();
            }

            var audio = GetComponent<AudioSource>();
            if (ChestCollisionSound != null)
            {
                audio.clip = ChestCollisionSound;
                audio.Play();
            }
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
            _rigidbody.AddRelativeTorque(0, 0, -(InclinationForce - _bonusRotationForce) * canMove);
        }
    }

    public void TurnLeft()
    {
        if (!IsShipCriticalAnglePassed())
        {
            _rigidbody.AddRelativeTorque(0, -RotationForce * canMove, 0);
            _rigidbody.AddRelativeTorque(0, 0, (InclinationForce - _bonusRotationForce) * canMove);
        }
    }

    public void CounterRightLeftRotation()
    {
        if (transform.localEulerAngles.z > 10 && transform.localEulerAngles.z < 350)
        {
            var add = transform.localEulerAngles.z < 180;
            _rigidbody.AddRelativeTorque(0, 0, (AntiSinkRightLeftForce + _bonusAntiSinkForce) * canMove * (add ? -1 : 1));
        }
    }

    public void CounterForwardBackRotation()
    {
        if (transform.localEulerAngles.x > 10 && transform.localEulerAngles.x < 350)
        {
            var add = transform.localEulerAngles.x < 180;
            _rigidbody.AddRelativeTorque((AntiSinkForwardBackForce + _bonusAntiSinkForce) * canMove * (add ? -1 : 1), 0, 0);
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
        else
        {
            _overturnedTimeStart = null;
        }

        return false;
    }

    public void ResetShipPosition()
    {
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        _overturnedTimeStart = null;

        var audio = GetComponent<AudioSource>();
        if (ChestCollisionSound != null)
        {
            audio.clip = OverturnSound;
            audio.Play();
        }
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

        OnPowerUpUsed?.Invoke(PowerUpReadyToLaunch);

        CurrentWorkingPowerUp = PowerUpReadyToLaunch;
        PowerUpReadyToLaunch = null;

        switch (CurrentWorkingPowerUp.Type)
        {
            case PowerUpType.Acceleration:
            {
                _bonusForwardForce = ForwardForceBonus;
                break;
            }

            case PowerUpType.Stabilizer:
            {
                _bonusAntiSinkForce = AntiSinkForceBonus;
                break;
            }

            case PowerUpType.Rotation:
            {
                _bonusRotationForce = RotationForceBonus;
                break;
            }
        }

        var audio = GetComponent<AudioSource>();
        if (ChestCollisionSound != null)
        {
            audio.clip = PowerUpEnabledSound;
            audio.Play();
        }

        _powerUpTimeStart = DateTime.Now;
    }

    public double GetPowerUpTimeLeft()
    {
        if (_powerUpTimeStart == null || CurrentWorkingPowerUp == null)
        {
            return 0;
        }

        return (_powerUpTimeStart.Value.AddSeconds(CurrentWorkingPowerUp.Time) - DateTime.Now).TotalSeconds;
    }

    public double GetTimeToReset()
    {
        if (_overturnedTimeStart != null)
        {
            return (_overturnedTimeStart.Value - DateTime.Now).TotalSeconds + SecondsBeforeReset;
        }

        return 0;
    }

    private void StopPowerUp()
    {
        _bonusForwardForce = 0;
        _bonusAntiSinkForce = 0;
        _bonusRotationForce = 0;
        CurrentWorkingPowerUp = null;

        var audio = GetComponent<AudioSource>();
        if (ChestCollisionSound != null)
        {
            audio.clip = PowerUpDisabledSound;
            audio.Play();
        }
    }
}
