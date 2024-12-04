using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private new Camera camera;
    private Rigidbody rb;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    [Range(1, 20)] public float speed = 5;

    [SerializeField] private float sensitivity;

    private void Awake()
    {
        if(camera == null) { camera = Camera.main; }
        rb = GetComponent<Rigidbody>();
        camera = GetComponent<Camera>();
        targetPosition = transform.position;
        targetRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        CameraMove();
        CameraRotation();
    }

    private void CameraMove()
    {
        Vector3 move = Vector3.zero;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.E)) { move += transform.up; }
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.Q)) { move -= transform.up; }
        if (Input.GetKey(KeyCode.A)) { move -= transform.right; }
        if (Input.GetKey(KeyCode.D)) { move += transform.right; }
        if (Input.GetKey(KeyCode.S)) { move -= transform.forward; }
        if (Input.GetKey(KeyCode.W)) { move += transform.forward; }

        rb.AddForce(move * speed * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void CameraRotation()
    {
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X") * sensitivity;
            float y = Input.GetAxis("Mouse Y") * sensitivity;
            targetRotation *= Quaternion.Euler(-y, x, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        }
    }


}