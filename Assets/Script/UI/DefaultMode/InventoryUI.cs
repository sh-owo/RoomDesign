using System;
using UnityEngine;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform slotHolder;
    [SerializeField] private GameObject slotPrefab;

    private List<GameObject> slots = new List<GameObject>();
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.onInventoryChanged += UpdateInventoryUI;
        
        // 초기 UI 설정
        UpdateInventoryUI();
    }

    private void Update()
    {
       UpdateInventoryUI(); 
    }

    void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.onInventoryChanged -= UpdateInventoryUI;
        }
    }

    private void UpdateInventoryUI()
    {
        ClearSlots();

        foreach (InventoryItem item in gameManager.Inventory)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotHolder);
            slots.Add(newSlot);

            InventorySlot slotUI = newSlot.GetComponent<InventorySlot>();
            if (slotUI != null)
            {
                slotUI.SetItem(item);
            }
        }
    }

    private void ClearSlots()
    {
        foreach (GameObject slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();
    }
}