using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct InventoryItem
{
    public string Name;
    public string Tags;
    public int Count;
    public Sprite Icon;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum Mode
    {
        Normal,      // 일반 모드
        Inventory,   // 아이템창 모드
        Shop,        // 상점 모드
        Game         // 게임 모드
    }
    
    public List<InventoryItem> Invertory = new List<InventoryItem>();

    public Mode CurrentMode = Mode.Normal;
    
    private void Awake()
    {
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, true);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}