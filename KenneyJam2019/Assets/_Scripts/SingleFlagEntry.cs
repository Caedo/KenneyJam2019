using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleFlagEntry : MonoBehaviour
{
    public MeshCollider Collider;
    public Rigidbody Rigidbody;
    public int CooldownAfterFall;

    private Vector3 _positionBackup;
    private Quaternion _rotationBackup;
    private DateTime? _collisionTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_collisionTime != null)
        {
            if ((DateTime.Now - _collisionTime.Value).TotalSeconds > CooldownAfterFall)
            {
                transform.position = _positionBackup;
                transform.rotation = _rotationBackup;
                _collisionTime = null;

                Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            _positionBackup = transform.position;
            _rotationBackup = transform.rotation;

            Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            _collisionTime = DateTime.Now;
        }
    }
}
