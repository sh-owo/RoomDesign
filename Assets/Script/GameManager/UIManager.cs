using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UIMode
{
    Normal,
    SceneMove,
    Shop,
    Game
}

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    private GameManager gameManager;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 새로 생성된 객체를 파괴
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
    }

    private UIMode currentMode;

    [Header("UI")]
    public GameObject normalUI;
    public GameObject sceneMoveUI;
    public GameObject shopUI;
    public GameObject gameUI;

    [Header("MoneyTMP")] public List<TMPro.TextMeshProUGUI> moneyTexts;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetMode(UIMode.Normal);
    }

    void Update()
    {
        if (currentMode != (UIMode)GameManager.Instance.CurrentMode)
        {
            SetMode((UIMode)GameManager.Instance.CurrentMode);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SetMode(UIMode.Normal);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetMode(UIMode.SceneMove);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetMode(UIMode.Shop);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetMode(UIMode.Game);

        foreach (var txt in moneyTexts)
        {
            txt.text = $"Money: {gameManager.Money.ToString()}";
        }
    }

    public void SetMode(UIMode mode)
    {
        currentMode = mode;

        if (normalUI != null) normalUI.SetActive(mode == UIMode.Normal);
        if (sceneMoveUI != null) sceneMoveUI.SetActive(mode == UIMode.SceneMove);
        if (shopUI != null) shopUI.SetActive(mode == UIMode.Shop);
        if (gameUI != null) gameUI.SetActive(mode == UIMode.Game);

        if (gameManager != null)
        {
            gameManager.SetMode((GameManager.Mode)mode);
        }
    }
}