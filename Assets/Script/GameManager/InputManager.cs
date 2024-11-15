using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask layerMask;
    
    private Vector3 pos;

    public event Action Onclicked, OnExit;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) { Onclicked?.Invoke(); }
        if (Input.GetKeyDown(KeyCode.Escape)) { OnExit?.Invoke(); }
    }

    public bool InOnUI() => EventSystem.current.IsPointerOverGameObject();
    
    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            pos = hit.point;
        }
        return pos;
    }
}
