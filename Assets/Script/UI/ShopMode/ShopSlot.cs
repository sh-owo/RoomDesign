using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public struct ShopItem
{
    public string Name;
    public string Tags;
    public int Price;
    public Sprite Icon;
    public GameObject Prefab;

    public ShopItem(string name, string tags, int price, Sprite icon, GameObject prefab)
    {
        Name = name;
        Tags = tags;
        Price = price;
        Icon = icon;
        Prefab = prefab;
    }
}
public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TMPro.TextMeshProUGUI priceText;
    [SerializeField] private TMPro.TextMeshProUGUI nameText;

    private ShopItem currentItem;
    private bool hasItem = false;

    public void SetItem(ShopItem item)
    {
        currentItem = item;
        hasItem = true;

        // 아이템 아이콘 설정
        if (iconImage != null)
        {
            iconImage.sprite = item.Icon;
            iconImage.enabled = true;
        }

        // 아이템 가격 표시
        if (priceText != null)
        {
            priceText.text = $"{item.Price}";
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
        if (hasItem && GameManager.Instance.CurrentMode == GameManager.Mode.Shop)
        {
            TryBuyItem();
        }
    }

    private void TryBuyItem()
    {
        // Check if the player has enough money
        if (GameManager.Instance.Money >= currentItem.Price)
        {
            // Deduct money
            GameManager.Instance.Money -= currentItem.Price;

            // Create an InventoryItem and add it to the inventory
            InventoryItem newItem = new InventoryItem(
                currentItem.Name,
                currentItem.Tags,
                1,  // Buy one item at a time
                currentItem.Icon,
                currentItem.Prefab
            );
            GameManager.Instance.AddItem(newItem);

            Debug.Log($"Purchased: {currentItem.Name} for {currentItem.Price}G");
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    // 슬롯 초기화
    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.enabled = false;
        }
        if (priceText != null)
        {
            priceText.text = "";
        }
        if (nameText != null)
        {
            nameText.text = "";
        }
        
        hasItem = false;
        currentItem = new ShopItem();
    }
}