using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TMPro.TextMeshProUGUI countText;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;

    public void SetItem(InventoryItem item)
    {
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
        
        if (countText != null)
        {
            countText.text = item.Count > 1 ? item.Count.ToString() : "";
        }


    }
}
