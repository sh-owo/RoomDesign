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
    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 새로 생성된 객체를 파괴
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
    }
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
    
    public void SetMode(Mode newMode)
    {
        CurrentMode = newMode;
    }
}