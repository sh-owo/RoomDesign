using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotHolder;
    
    private List<GameObject> slots = new List<GameObject>();
    private GameManager gameManager;
    private int currentTagIndex = 0;
    private Tag[] tagValues;

    private void Start()
    {
        gameManager = GameManager.Instance;
        if (gameManager != null)
        {
            gameManager.onInventoryChanged += UpdateInventoryUI;
        }

        // Get all possible tag values
        tagValues = (Tag[])System.Enum.GetValues(typeof(Tag));
        UpdateInventoryUI();
    }

    private void Update()
    {
        // Change category with X and Z keys
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentTagIndex++;
            if (currentTagIndex >= tagValues.Length)
                currentTagIndex = 0;
            UpdateInventoryUI();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            currentTagIndex--;
            if (currentTagIndex < 0)
                currentTagIndex = tagValues.Length - 1;
            UpdateInventoryUI();
        }
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

        // Get current tag category
        Tag currentTag = tagValues[currentTagIndex];
        
        // Filter items by current tag
        var filteredItems = gameManager.Inventory
            .Where(item => System.Enum.TryParse(item.Tags, out Tag itemTag) && itemTag == currentTag)
            .ToList();

        // Create slots for filtered items
        foreach (InventoryItem item in filteredItems)
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

    // Helper method to get current category index
    public int GetCurrentTagIndex()
    {
        return currentTagIndex;
    }
}