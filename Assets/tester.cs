using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private Sprite testSprite;  // Inspector에서 보이는 Slot Example을 여기에 할당
    [SerializeField] private Sprite another;

    void Start()
    {
        // 게임 시작할 때 테스트용 아이템 추가
        GameManager.Instance.AddItem("Test Item 1", "Test", 13, testSprite);
        GameManager.Instance.AddItem("Test Item 2", "Test", 12, another);
    }

    void Update()
    {
        // 테스트용: T 키를 누르면 아이템 추가
        if (Input.GetKeyDown(KeyCode.T))
        {
            GameManager.Instance.AddItem("New Item", "Test", 1, testSprite);
            GameManager.Instance.AddItem("Test Item 2", "Test", 1, another);
            
        }
    }
}