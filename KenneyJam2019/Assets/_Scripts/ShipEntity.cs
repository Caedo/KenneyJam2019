using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ShipEntity : MonoBehaviour
{
    public float ForwardForce;
    public float BackwardForce;
    public float RotationForce;
    public float InclinationForce;
    public float AntiSinkRightLeftForce;
    public float AntiSinkForwardBackForce;

    private Rigidbody _rigidbody;
    private bool _collisionDetected;

    [HideInInspector]
    public float canMove = 1; // 0 as not, 1 as sail to the end of th world!

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_collisionDetected)
        {
            if (Input.GetKey(KeyCode.W))
            {
                MoveForward();
            }

            if (Input.GetKey(KeyCode.S))
            {
                Brake();
            }
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            CounterRightLeftRotation();
        }

        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            CounterForwardBackRotation();
        }

        if (Input.GetKey(KeyCode.A))
        {
            TurnLeft();
        }

        if (Input.GetKey(KeyCode.D))
        {
            TurnRight();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _collisionDetected = true;
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
        _rigidbody.AddRelativeForce(Vector3.forward * -ForwardForce * canMove);
    }

    public void Brake()
    {
        if (_rigidbody.velocity.z < 0)
        {
            _rigidbody.AddRelativeForce(Vector3.forward * BackwardForce * canMove);
        }
    }

    public void TurnRight()
    {
        _rigidbody.AddRelativeTorque(0, RotationForce * canMove, 0);
        _rigidbody.AddRelativeTorque(0, 0, -InclinationForce * canMove);
    }

    public void TurnLeft()
    {
        _rigidbody.AddRelativeTorque(0, -RotationForce * canMove, 0);
        _rigidbody.AddRelativeTorque(0, 0, InclinationForce * canMove);
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
}
