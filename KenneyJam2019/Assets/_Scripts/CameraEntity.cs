using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEntity : MonoBehaviour {
    public GameObject CameraTarget;
    private Vector3 _velocity;
    private float _rotationVel;

    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (CameraTarget == null) return;

        transform.position = Vector3.SmoothDamp(transform.position, CameraTarget.transform.position, ref _velocity, 0.3f);
        float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, CameraTarget.transform.eulerAngles.y, ref _rotationVel, 0.3f);

        transform.eulerAngles = new Vector3(CameraTarget.transform.eulerAngles.x, rotationY, CameraTarget.transform.eulerAngles.z);
    }
}