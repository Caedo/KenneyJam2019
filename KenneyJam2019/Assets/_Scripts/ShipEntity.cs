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

    private Rigidbody _rigidbody;
    private bool _collisionDetected;

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
                _rigidbody.AddRelativeForce(Vector3.forward * -ForwardForce);
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (_rigidbody.velocity.z < 0)
                {
                    _rigidbody.AddRelativeForce(Vector3.forward * BackwardForce);
                }
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody.AddRelativeTorque(0, -RotationForce, 0);
            _rigidbody.AddRelativeTorque(0, 0, InclinationForce);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.AddRelativeTorque(0, RotationForce, 0);
            _rigidbody.AddRelativeTorque(0, 0, -InclinationForce);
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
}
