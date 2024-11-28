using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder;
    private List<GameObject> slots = new List<GameObject>();
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.onInventoryChanged += UpdateInventoryUI;
        }

        UpdateInventoryUI();
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.onInventoryChanged -= UpdateInventoryUI;
        }
    }

    public void UpdateInventoryUI()
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