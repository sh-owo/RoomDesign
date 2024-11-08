using System;
using UnityEngine;
using System.Collections.Generic;

public class BuildManager : MonoBehaviour
{
    //TODO : https://youtu.be/i9W1kqUinIs?si=3cF00zsGviBhG3iu&t=556
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Grid grid;
    
    [SerializeField] private ObjectDatabaseSO database;
    private int currentIndex = -1;

    private void Update()
    {
       Vector3 mousePos = inputManager.GetMousePosition();
       mouseIndicator.transform.position = mousePos;
    }
}