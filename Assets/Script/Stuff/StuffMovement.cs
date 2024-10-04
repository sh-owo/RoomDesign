using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffMovement : MonoBehaviour
{
    private Vector3 movePos;
    private Renderer objectRenderer;
    private Rigidbody rb;

    private bool hasColided = false;
    private bool collisionDetected = false;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; 
    }

    void FixedUpdate()
    {
        if (collisionDetected && !hasColided)
        {
            transform.position += movePos * Time.fixedDeltaTime;
        }
        if(collisionDetected && rb.velocity.magnitude > 9.81f)
        {
            Freeze();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            movePos = -contact.normal * 0.001f; 
            collisionDetected = true; 
        }
    }

    private void OnCollisionExit(Collision other)
    {
        collisionDetected = false;
        if (!hasColided)
        {
            Freeze();
            hasColided = true;
        }
    }

    private void Freeze()
    {
        rb.isKinematic = true; 
        rb.isKinematic = false; 
    }
}