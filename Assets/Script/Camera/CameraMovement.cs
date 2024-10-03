using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Camera camera;
    private float movementSpeed = 5f;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private bool isColiding = false;
    
    [SerializeField] private float sensitivity;
    
    

    private void Awake()
    {
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

        if (isColiding) { targetPosition += move * movementSpeed / 3 * Time.deltaTime;}
        else{targetPosition += move * movementSpeed * Time.deltaTime;}
        float lerpvalue = Time.deltaTime * 5;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5);
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

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.transform.name);
        isColiding = true;
    }
}