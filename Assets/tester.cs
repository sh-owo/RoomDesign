using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private Sprite testSprite;  // Inspector에서 보이는 Slot Example을 여기에 할당
    [SerializeField] private Sprite another;

    void Start()
    {
        // 게임 시작할 때 테스트용 아이템 추가
        InventoryItem item1 = new InventoryItem("Test Item 1", "Test", 13, testSprite, null);
        InventoryItem item2 = new InventoryItem("Test Item 2", "Test", 12, another, null);
        GameManager.Instance.AddItem(item1);
        GameManager.Instance.AddItem(item2);
    }

    void Update()
    {
        // 테스트용: T 키를 누르면 아이템 추가
        if (Input.GetKeyDown(KeyCode.T))
        {
            InventoryItem newItem = new InventoryItem("New Item", "Test", 1, testSprite, null);
            InventoryItem anotherItem = new InventoryItem("Test Item 2", "Test", 1, another, null);
            GameManager.Instance.AddItem(newItem);
            GameManager.Instance.AddItem(anotherItem);
        }
    }
}