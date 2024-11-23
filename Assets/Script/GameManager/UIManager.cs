using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // UI 상태를 나타내는 열거형
    public enum UIMode
    {
        Normal,      // 일반 모드
        Inventory,   // 아이템창 모드
        Shop,        // 상점 모드
        Game         // 게임 모드
    }

    // 현재 UI 상태
    private UIMode currentMode;

    // 각 모드의 UI를 참조하기 위한 GameObject 변수
    public GameObject normalUI;
    public GameObject inventoryUI;
    public GameObject shopUI;
    public GameObject gameUI;

    void Start()
    {
        // 초기 모드를 Normal로 설정
        SetMode(UIMode.Normal);
    }

    void Update()
    {
        // 키 입력으로 모드 전환 테스트 (임시 입력 처리)
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetMode(UIMode.Normal);      // 일반 모드
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetMode(UIMode.Inventory);   // 아이템창 모드
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetMode(UIMode.Shop);        // 상점 모드
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetMode(UIMode.Game);        // 게임 모드
    }

    // 모드를 설정하는 메서드
    public void SetMode(UIMode mode)
    {
        currentMode = mode;

        // 모든 UI를 비활성화한 후, 해당 모드의 UI만 활성화
        if (normalUI != null) normalUI.SetActive(mode == UIMode.Normal);
        if (inventoryUI != null) inventoryUI.SetActive(mode == UIMode.Inventory);
        if (shopUI != null) shopUI.SetActive(mode == UIMode.Shop);
        if (gameUI != null) gameUI.SetActive(mode == UIMode.Game);

        Debug.Log("현재 모드: " + mode);
    }

    // 현재 모드를 반환 (필요 시 외부에서 호출 가능)
    public UIMode GetCurrentMode()
    {
        return currentMode;
    }
}