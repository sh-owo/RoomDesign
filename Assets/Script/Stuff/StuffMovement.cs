using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffMovement : MonoBehaviour
{
    private Vector3 movePos;
    private Renderer objectRenderer;
    private Rigidbody rb;

    private bool isColliding = false;
    private bool hasMoved = false;
    private bool collisionDetected = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        // objectRenderer.enabled = false;
    }

    void FixedUpdate()
    {
        if (collisionDetected)
        {
            transform.position += movePos * Time.fixedDeltaTime;
            collisionDetected = false; 
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            isColliding = true;
            movePos = -contact.normal * 0.001f;
            collisionDetected = true; 
        }
    }

    private void OnCollisionExit(Collision other)
    {
        isColliding = false;
        // objectRenderer.enabled = true;
        rb.isKinematic = true;
        rb.isKinematic = false;
    }
}