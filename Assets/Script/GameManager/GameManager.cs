using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Mode
    {
        Normal,
        MoveManager,
        Shop,
        MapManager,
        Game
    }

    public static GameManager Instance { get; private set; }

    private Mode currentMode = Mode.Normal;
    public Mode CurrentMode => currentMode;

    public GameObject selectedPrefab { get; set; }

    [SerializeField] private ObjectDatabaseSO objectDatabase;
    private List<InventoryItem> inventory = new List<InventoryItem>();
    public IReadOnlyList<InventoryItem> Inventory => inventory;
    [SerializeField] private GameObject buildManager;

    void Update()
    {
        buildManager.SetActive(true);
    }

    public int money = 1000;
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

    /*public void SetMode(Mode newMode)
    {
        if (currentMode != newMode)
        {
            currentMode = newMode;
            Debug.Log($"Game mode changed to: {newMode}");

            if (newMode != Mode.Game)
            {
                selectedPrefab = null;
            }
        }

        if (buildManager != null)
        {
            BuildManager manager = buildManager.GetComponent<BuildManager>();
            if (currentMode == Mode.Normal || currentMode == Mode.Game)
            {
                buildManager.SetActive(true);
            }
            else
            {
                manager.Disable();
                buildManager.SetActive(false);
            }
        }
    }*/
    public void SetMode(Mode newMode)
    {
        if (currentMode != newMode)
        {
            currentMode = newMode;
            Debug.Log($"Game mode changed to: {newMode}");

            if (newMode != Mode.Game)
            {
                selectedPrefab = null;
            }
        }

        if (buildManager != null)
        {
            buildManager.SetActive(true);
        }
    }

    public void AddItem(InventoryItem item)
    {
        var existingItem = inventory.Find(i => i.Name == item.Name);
        if (existingItem != null)
        {
            existingItem.AddCount(item.Count);
        }
        else
        {
            inventory.Add(item);
        }

        onInventoryChanged?.Invoke();
        Debug.Log($"Added {item.Count} {item.Name}(s) to inventory. Total: {GetItemCount(item.Name)}");
    }

    public bool RemoveItem(string name, int count = 1)
    {
        var item = inventory.Find(i => i.Name == name);
        if (item != null && item.Count >= count)
        {
            item.RemoveCount(count);
            if (item.Count == 0)
            {
                inventory.Remove(item);
            }

            onInventoryChanged?.Invoke();
            Debug.Log($"Removed {count} {name}(s) from inventory. Remaining: {GetItemCount(name)}");
            return true;
        }

        Debug.LogWarning($"Failed to remove {count} {name}(s) from inventory: insufficient quantity");
        return false;
    }

    public int GetItemCount(string name)
    {
        var item = inventory.Find(i => i.Name == name);
        return item != null ? item.Count : 0;
    }

    public bool HasItem(string name, int count = 1)
    {
        return GetItemCount(name) >= count;
    }

    public InventoryItem GetItem(string name)
    {
        return inventory.Find(i => i.Name == name);
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
        Debug.Log("Game state saved");
    }

    public void LoadGameState()
    {
        Debug.Log("Game state loaded");
    }

    private void OnApplicationQuit()
    {
        SaveGameState();
    }

    private void OnValidate()
    {
        if (objectDatabase == null)
        {
            Debug.LogWarning("ObjectDatabase is not assigned to GameManager!");
        }
    }
}