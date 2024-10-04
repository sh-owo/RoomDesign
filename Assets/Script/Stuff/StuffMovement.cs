using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffMovement : MonoBehaviour
{
    private Vector3 movePos;
    private bool isColliding = false;
    private bool isOutside = true;
    private Renderer objectRenderer;
    private Rigidbody rb;
    
    
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();
        objectRenderer.enabled = false; 
    }

    
    void FixedUpdate()
    {
        if (isColliding) { transform.position += movePos * Time.fixedDeltaTime; Debug.Log("Moved"+movePos);}
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isOutside)
        {  
            foreach (ContactPoint contact in other.contacts)
            {
                isColliding = true;
                movePos = -contact.normal; 
            }

        }
        if(other.gameObject.CompareTag("Wall"))
        {
            // Debug.Log("Wall");
            objectRenderer.enabled = true; 
        }
    }

    private void OnCollisionExit(Collision other)
    {
        isOutside = true;
        isColliding = false;
        objectRenderer.enabled = true;
        rb.isKinematic = true;
    }
    
    
    
}