using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TMPro.TextMeshProUGUI countText;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;
    [SerializeField] private GameObject itemPrefab; // 해당 슬롯의 아이템 프리팹

    private InventoryItem currentItem;
    private bool hasItem = false;

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        hasItem = true;

        // 아이템 아이콘 설정
        if (iconImage != null)
        {
            iconImage.sprite = item.Icon;
            iconImage.enabled = true;
        }

        // 아이템 개수 표시
        if (countText != null)
        {
            countText.text = item.Count > 1 ? item.Count.ToString() : "";
        }

        // 아이템 이름 표시
        if (nameText != null)
        {
            nameText.text = item.Name;
        }
    }

    // 아이템 슬롯 클릭 처리
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Slot clicked!"); // 클릭이 감지되는지 확인하기 위한 디버그 로그
        
        if (hasItem && GameManager.Instance != null && 
            GameManager.Instance.CurrentMode == GameManager.Mode.Inventory)
        {
            Debug.Log($"Selected item: {currentItem.Name} with tags: {currentItem.Tags}");
            
            if (itemPrefab != null)
            {
                GameManager.Instance.SelectedPrefab = itemPrefab;
                Debug.Log("Prefab selected!"); // 프리팹이 선택되었는지 확인
            }
            else
            {
                Debug.LogWarning("No prefab assigned to this slot!");
            }
        }
        else
        {
            Debug.Log($"Click detected but conditions not met: HasItem={hasItem}, " +
                      $"GameManager null={GameManager.Instance == null}"); // 조건 확인을 위한 디버그 로그
        }
    }

    // 슬롯 초기화
    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.enabled = false;
        }
        if (countText != null)
        {
            countText.text = "";
        }
        if (nameText != null)
        {
            nameText.text = "";
        }
        
        hasItem = false;
        currentItem = new InventoryItem();
    }

    // 현재 아이템의 프리팹 설정
    public void SetItemPrefab(GameObject prefab)
    {
        itemPrefab = prefab;
    }
}