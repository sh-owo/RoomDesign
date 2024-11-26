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
        if (hasItem && GameManager.Instance.CurrentMode == GameManager.Mode.Inventory)
        {
            // 현재 선택된 아이템 정보를 GameManager에 저장하는 변수를 추가해야 합니다.
            Debug.Log($"Selected item: {currentItem.Name} with tags: {currentItem.Tags}");
            
            // GameManager에 선택된 아이템 프리팹을 저장하기 위한 속성을 추가합니다.
            if (itemPrefab != null)
            {
                // GameManager에 SelectedPrefab 속성을 추가해야 합니다.
                // GameManager.Instance.SelectedPrefab = itemPrefab;
            }
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