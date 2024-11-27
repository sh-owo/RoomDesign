using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private Transform slotHolder;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private ObjectDatabaseSO objectDatabase;

    private List<ShopSlot> shopSlots = new List<ShopSlot>();

    private void Start()
    {
        InitializeShop();
    }
    

    private void InitializeShop()
    {
        // 기존 슬롯들 제거
        ClearSlots();

        // 데이터베이스의 각 아이템에 대해 슬롯 생성
        foreach (ObjectData objectData in objectDatabase.ObjectsData)
        {
            // 슬롯 생성
            GameObject newSlotObj = Instantiate(slotPrefab, slotHolder);
            ShopSlot shopSlot = newSlotObj.GetComponent<ShopSlot>();
            shopSlots.Add(shopSlot);

            // 태그들을 string으로 변환
            string tagString = ConvertTagsToString(objectData.Tags);

            // ShopItem 생성
            ShopItem shopItem = new ShopItem(
                objectData.Name,
                tagString,
                objectData.Price,
                objectData.Icon
            );

            // 슬롯에 아이템 설정
            shopSlot.SetItem(shopItem);
        }
    }

    private string ConvertTagsToString(List<Tag> tags)
    {
        string result = "";
        for (int i = 0; i < tags.Count; i++)
        {
            result += tags[i].ToString();
            if (i < tags.Count - 1)
            {
                result += ", ";
            }
        }
        return result;
    }

    private void ClearSlots()
    {
        foreach (ShopSlot slot in shopSlots)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }
        shopSlots.Clear();
    }

    // 상점 UI 표시
    public void Show()
    {
        shopPanel.SetActive(true);
        GameManager.Instance.SetMode(GameManager.Mode.Shop);
    }

    // 상점 UI 숨기기
    public void Hide()
    {
        shopPanel.SetActive(false);
        GameManager.Instance.SetMode(GameManager.Mode.Normal);
    }

    // 상점 UI 토글
    public void Toggle()
    {
        if (shopPanel.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
}