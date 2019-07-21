﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    Camera cam;

    public Transform target;
    public Vector3 offset;

    public float cameraDistance = 8f;
    public float minDistance = 5f;
    public float maxDistance = 10f;
    public float zoomSpeed = 4f;

    void Start() {
        cam = GetComponentInChildren<Camera>();
    }

    void Update() {
        // Zoom the camera.
        cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        cameraDistance = Mathf.Clamp(cameraDistance, minDistance, maxDistance);
    }

    void LateUpdate() {
        // Move camera forward Player.
        transform.position = target.position;
        cam.transform.position = transform.position - offset * cameraDistance;
        //cam.transform.LookAt(transform.position);
    }
}