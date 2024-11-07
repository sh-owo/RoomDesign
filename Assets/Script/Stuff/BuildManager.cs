using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public List<GameObject> stuffList;
    private GameObject selectedObject;
    
    private Vector3 pos;
    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;
    

    public float gridSize;
    private bool gridOn;
    public bool gridToggle;

    void Update()
    {
        selectedObject = stuffList[0];
       if(selectedObject != null)
       {
           if (gridOn)
           {
               selectedObject.transform.position = new Vector3(RoundToNearestGrid(pos.x), RoundToNearestGrid(pos.y), RoundToNearestGrid(pos.z));
               Debug.Log("grid");
           }
           else { selectedObject.transform.position = pos; }

           if (Input.GetKeyDown(KeyCode.F))
           {
               Debug.Log("Placed");
               SelectObject(0);
           }
       }
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            pos = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        selectedObject = Instantiate(stuffList[index], pos, Quaternion.identity);
    }
    
    public void PlaceObject()
    {
        selectedObject = null;
    }

    public void ToggleGrid()
    {
        
    }
    
    private float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if (xDiff > gridSize / 2)
        {
            pos += gridSize;
        }
        return pos;
    }
}