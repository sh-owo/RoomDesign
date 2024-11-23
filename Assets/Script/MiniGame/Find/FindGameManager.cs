using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGameManager : MonoBehaviour
{
    public static FindGameManager Instance { get; private set; }
    
    public List<GameObject> spawnPos = new List<GameObject>();

    public float speed = 15f;
    public float rotationSpeed = 220f;
    
    public bool isGameStart = false;
    public bool isGameEnd = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; 
        }
    }
    
}
