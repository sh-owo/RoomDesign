using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FindGameManager : MonoBehaviour
{
    public static FindGameManager Instance { get; private set; }
    
    public List<GameObject> spawnPos = new List<GameObject>();

    public float speed = 15f;
    public float rotationSpeed = 220f;
    
    public bool isGameStart = false;
    public bool isGameEnd = false;
    public bool isPlayerWon = false;

    [SerializeField] private GameObject messageUI;
    
    public Vector3 StartPos { get; private set; }
    public int prize { get; private set; }
    
    

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
    
    private void Start()
    {
        UIManager.Instance.SetMode(UIMode.Game);
        prize = 100 * UnityEngine.Random.Range(3, 7);
        int random = UnityEngine.Random.Range(0, spawnPos.Count - 1);
        StartPos = spawnPos[random].transform.position;
        
        StartCoroutine(StartGame(3));
    }
    
    private IEnumerator StartGame(int second)
    {
        FindGameUI ui = messageUI.GetComponent<FindGameUI>();
        for (int i = second; i > 0; i--)
        {
            Debug.Log($"Game starts in {i}...");
            ui.LoadUI(i);
            
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Game Start!");
        ui.Start();
        
        isGameStart = true;
    }
}
