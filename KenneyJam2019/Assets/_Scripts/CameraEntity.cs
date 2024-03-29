﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEntity : MonoBehaviour {
    public GameObject CameraTarget;
    public float MinDistanceToTerrain;

    private Vector3 _velocity;
    private float _rotationVel;

    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        if (CameraTarget == null) return;

        RaycastHit hit;
        var offset = 0f;

        /*if (Physics.Raycast(CameraTarget.transform.position, Vector3.down, out hit))
        {
            if (hit.collider.gameObject.tag == "Ground" && hit.distance <= MinDistanceToTerrain)
            {
                offset = MinDistanceToTerrain;
                Debug.Log("X " + hit.distance);
            }
        }*/

        var fixedCameraTarget = CameraTarget.transform.position;
        if(Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            if (hit.collider.gameObject.tag == "Ground" && hit.distance <= MinDistanceToTerrain)
            {
                fixedCameraTarget += new Vector3(0, MinDistanceToTerrain - hit.distance, 0);
            }
        }
        else if (Physics.Raycast(transform.position, Vector3.up, out hit))
        {
            if (hit.collider.gameObject.tag == "Ground" && hit.distance <= MinDistanceToTerrain)
            {
                fixedCameraTarget += new Vector3(0, MinDistanceToTerrain + hit.distance, 0);
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, fixedCameraTarget, ref _velocity, 0.3f);
        var rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, CameraTarget.transform.eulerAngles.y, ref _rotationVel, 0.3f);

        transform.eulerAngles = new Vector3(CameraTarget.transform.eulerAngles.x, rotationY, CameraTarget.transform.eulerAngles.z);


    }
}