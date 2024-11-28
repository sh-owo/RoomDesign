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
    public GameObject Prefab;

    public InventoryItem(string name, string tags, int count, Sprite icon, GameObject prefab)
    {
        Name = name;
        Tags = tags;
        Count = count;
        Icon = icon;
        Prefab = prefab;
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
    
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    // 현재 모드 관리
    private Mode currentMode = Mode.Normal;
    public Mode CurrentMode => currentMode;

    // 선택된 프리팹 관리
    public GameObject selectedPrefab;


    // 인벤토리 및 게임 상태 관리
    [SerializeField] private ObjectDatabaseSO objectDatabase;
    private List<InventoryItem> inventory = new List<InventoryItem>();
    public IReadOnlyList<InventoryItem> Inventory => inventory;
    
    private int money = 1000;
    public int Money
    {
        get => money;
        set
        {
            if (money != value)
            {
                money = value;
                onMoneyChanged?.Invoke(money);
            }
        }
    }

    // 이벤트
    public event System.Action onInventoryChanged;
    public event System.Action<int> onMoneyChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitializeGame();
    }

    private void InitializeGame()
    {
        inventory.Clear();
        Money = 1000;
        currentMode = Mode.Normal;
        selectedPrefab = null;
    }

    public void SetMode(Mode newMode)
    {
        if (currentMode != newMode)
        {
            currentMode = newMode;
            Debug.Log($"Game mode changed to: {newMode}");
            
            // Game 모드가 아닐 때는 선택된 프리팹 초기화
            if (newMode != Mode.Game)
            {
                selectedPrefab = null;
            }
        }
    }

    public void AddItem(string name, string tags, int count, Sprite icon, GameObject prefab)
    {
        if (count <= 0)
        {
            Debug.LogWarning($"Cannot add {count} items to inventory");
            return;
        }

        int existingIndex = inventory.FindIndex(item => item.Name == name);
        
        if (existingIndex != -1)
        {
            var existingItem = inventory[existingIndex];
            existingItem.Count += count;
            inventory[existingIndex] = existingItem;
        }
        else
        {
            inventory.Add(new InventoryItem(name, tags, count, icon, prefab));
        }

        onInventoryChanged?.Invoke();
        Debug.Log($"Added {count} {name}(s) to inventory. Total: {GetItemCount(name)}");
    }

    public bool RemoveItem(string name, int count = 1)
    {
        if (count <= 0)
        {
            Debug.LogWarning($"Cannot remove {count} items from inventory");
            return false;
        }

        int index = inventory.FindIndex(item => item.Name == name);
        
        if (index != -1)
        {
            var item = inventory[index];
            if (item.Count >= count)
            {
                item.Count -= count;

                if (item.Count <= 0)
                {
                    inventory.RemoveAt(index);
                }
                else
                {
                    inventory[index] = item;
                }

                onInventoryChanged?.Invoke();
                Debug.Log($"Removed {count} {name}(s) from inventory. Remaining: {GetItemCount(name)}");
                return true;
            }
        }
        
        Debug.LogWarning($"Failed to remove {count} {name}(s) from inventory: insufficient quantity");
        return false;
    }

    public int GetItemCount(string name)
    {
        int index = inventory.FindIndex(item => item.Name == name);
        return index != -1 ? inventory[index].Count : 0;
    }

    public bool HasItem(string name, int count = 1)
    {
        return GetItemCount(name) >= count;
    }

    public InventoryItem? GetItem(string name)
    {
        int index = inventory.FindIndex(item => item.Name == name);
        return index != -1 ? inventory[index] : (InventoryItem?)null;
    }

    public bool AddMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Cannot add negative or zero amount of money: {amount}");
            return false;
        }
        
        Money += amount;
        Debug.Log($"Added {amount} money. New balance: {Money}");
        return true;
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning($"Cannot spend negative or zero amount of money: {amount}");
            return false;
        }

        if (Money < amount)
        {
            Debug.LogWarning($"Cannot spend {amount} money: insufficient funds (current balance: {Money})");
            return false;
        }

        Money -= amount;
        Debug.Log($"Spent {amount} money. Remaining balance: {Money}");
        return true;
    }

    public bool HasEnoughMoney(int amount)
    {
        return Money >= amount;
    }

    public void SaveGameState()
    {
        // TODO: 게임 상태 저장 구현
        Debug.Log("Game state saved");
    }

    public void LoadGameState()
    {
        // TODO: 게임 상태 로드 구현
        Debug.Log("Game state loaded");
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    // 디버그용 메서드
    private void OnValidate()
    {
        if (objectDatabase == null)
        {
            Debug.LogWarning("ObjectDatabase is not assigned to GameManager!");
        }
    }
}