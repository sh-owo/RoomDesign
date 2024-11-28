using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TMPro.TextMeshProUGUI countText;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private GameObject itemPrefab;

    private InventoryItem currentItem;
    private InventoryUI inventoryUI;
    private bool hasItem = false;

    void Awake()
    {
        inventoryUI = GetComponentInParent<InventoryUI>();
        if (inventoryUI == null)
        {
            Debug.LogWarning("InventoryUI component not found in parents!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"OnPointerClick - Current Mode: {GameManager.Instance.CurrentMode}, HasItem: {hasItem}");

        if (hasItem && GameManager.Instance != null)
        {
            Debug.Log($"Selected item: {currentItem.Name} with tags: {currentItem.Tags}");

            if (itemPrefab != null)
            {
                // 프리팹 설정
                GameManager.Instance.selectedPrefab = itemPrefab;

                // 게임 모드로 전환
                GameManager.Instance.SetMode(GameManager.Mode.Game);
                UIManager.Instance.SetMode(UIMode.Game);

                Debug.Log($"Prefab selected and mode changed to Game");
            }
            else
            {
                Debug.LogWarning("No prefab assigned to this slot!");
            }
        }

        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI();
        }
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        hasItem = true;
        itemPrefab = item.Prefab;  // 아이템의 프리팹 설정

        if (iconImage != null)
        {
            iconImage.sprite = item.Icon;
            iconImage.enabled = true;
        }

        if (countText != null)
        {
            countText.text = item.Count > 1 ? item.Count.ToString() : "";
        }

        if (nameText != null)
        {
            nameText.text = item.Name;
        }
    }
}