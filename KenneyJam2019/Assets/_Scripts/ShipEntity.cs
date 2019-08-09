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
    private BoxCollider _collider;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    void Update()
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

        var ratio = 1f;
        var zSpeed = Math.Abs(_rigidbody.velocity.z);
        if (zSpeed < 10)
        {
            ratio = Math.Abs(zSpeed) < 0.001f ? 0 : 1 / (10f - Math.Abs(_rigidbody.velocity.z));
        }

        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody.AddRelativeTorque(Vector3.forward * RotationForce * ratio);
            _rigidbody.AddRelativeTorque(0, -InclinationForce * ratio, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _rigidbody.AddRelativeTorque(Vector3.forward * -RotationForce * ratio);
            _rigidbody.AddRelativeTorque(0, InclinationForce * ratio, 0);
        }
    }
}
