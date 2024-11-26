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

    public InventoryItem(string name, string tags, int count, Sprite icon)
    {
        Name = name;
        Tags = tags;
        Count = count;
        Icon = icon;
    }
}

public class GameManager : MonoBehaviour
{
    public enum Mode
    {
        Normal,      // 일반 모드
        Inventory,   // 아이템창 모드
        Shop,        // 상점 모드
        Game         // 게임 모드
    }
    public static GameManager Instance { get; private set; }
    public GameObject SelectedPrefab { get; set; }

    public void SetSelectedPrefab(GameObject prefab)
    {
        SelectedPrefab = prefab;
    }
    
    [SerializeField] private ObjectDatabaseSO objectDatabase;
    public List<InventoryItem> Inventory = new List<InventoryItem>();
    public Mode CurrentMode = Mode.Normal;
    public int Money = 1000;

    // 인벤토리 변경 이벤트 추가
    public event System.Action onInventoryChanged;
    
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

    // 아이템 추가 메서드
    public void AddItem(string name, string tags, int count, Sprite icon)
    {
        // 이미 같은 이름의 아이템이 있는지 확인
        int existingIndex = Inventory.FindIndex(item => item.Name == name);
        
        if (existingIndex != -1)
        {
            // 기존 아이템이 있다면 개수만 증가
            InventoryItem existingItem = Inventory[existingIndex];
            existingItem.Count += count;
            Inventory[existingIndex] = existingItem;
        }
        else
        {
            // 새 아이템 추가
            Inventory.Add(new InventoryItem(name, tags, count, icon));
        }

        // 이벤트 발생
        onInventoryChanged?.Invoke();
    }

    // 아이템 제거 메서드
    public void RemoveItem(string name, int count = 1)
    {
        int index = Inventory.FindIndex(item => item.Name == name);
        
        if (index != -1)
        {
            InventoryItem item = Inventory[index];
            item.Count -= count;

            if (item.Count <= 0)
            {
                Inventory.RemoveAt(index);
            }
            else
            {
                Inventory[index] = item;
            }

            onInventoryChanged?.Invoke();
        }
    }

    // 아이템 개수 확인 메서드
    public int GetItemCount(string name)
    {
        int index = Inventory.FindIndex(item => item.Name == name);
        return index != -1 ? Inventory[index].Count : 0;
    }

    // 모드 변경 메서드
    public void SetMode(Mode newMode)
    {
        CurrentMode = newMode;
        
        // 인벤토리 모드일 때는 커서 표시
        Cursor.visible = (newMode == Mode.Inventory || newMode == Mode.Shop);
    }
}