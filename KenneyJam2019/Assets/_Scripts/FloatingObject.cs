using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public Transform CollisionPointsRoot;
    public int DisplacementForce;
    public float DragRatio;

    private Rigidbody _rigidbody;
    private BoxCollider _collider;
    private List<Transform> _collisionPoints;

    private bool _collisionWithWater;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _collisionPoints = new List<Transform>();

        for (var i = 0; i < 3; i++)
        {
            var origin = new Vector3(_collider.bounds.min.x, i * 10, _collider.bounds.min.z);

            _collisionPoints.Add(Instantiate(CollisionPointsRoot, origin, Quaternion.identity, CollisionPointsRoot));
            _collisionPoints.Add(Instantiate(CollisionPointsRoot, origin + new Vector3(_collider.size.x, 0, 0), Quaternion.identity, CollisionPointsRoot));
            _collisionPoints.Add(Instantiate(CollisionPointsRoot, origin + new Vector3(0, 0, _collider.size.z), Quaternion.identity, CollisionPointsRoot));
            _collisionPoints.Add(Instantiate(CollisionPointsRoot, origin + new Vector3(_collider.size.x, 0, _collider.size.z), Quaternion.identity, CollisionPointsRoot));
            _collisionPoints.Add(Instantiate(CollisionPointsRoot, origin + new Vector3(0, 0, _collider.size.z / 2), Quaternion.identity, CollisionPointsRoot));
            _collisionPoints.Add(Instantiate(CollisionPointsRoot, origin + new Vector3(_collider.size.x, 0, _collider.size.z / 2), Quaternion.identity, CollisionPointsRoot));
        }
    }

    void Update()
    {
        foreach (var point in _collisionPoints)
        {
            RaycastHit hit;
            var ray = new Ray(point.transform.position, Vector3.up);
            var waterLayer = LayerMask.GetMask("Water");

            if (Physics.Raycast(ray, out hit, DisplacementForce, waterLayer) && _rigidbody.velocity.y <= 0)
            {
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y / DragRatio, _rigidbody.velocity.z);
                _rigidbody.angularVelocity = new Vector3(_rigidbody.angularVelocity.x, _rigidbody.angularVelocity.y / DragRatio, _rigidbody.angularVelocity.z);
            }
            else
            {
                _rigidbody.AddForceAtPosition(new Vector3(0, -5000, 0), point.position, ForceMode.Force);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "WaterPlane")
        {
            _collisionWithWater = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "WaterPlane")
        {
            _collisionWithWater = false;
        }
    }
}
