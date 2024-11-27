using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UIMode
    {
        Normal,
        Inventory,
        Shop,
        Game
    }

    private UIMode currentMode;
    
    [Header("UI")]
    public GameObject normalUI;
    public GameObject inventoryUI;
    public GameObject shopUI;
    public GameObject gameUI;

    [Header("MoneyTMP")] public List<TMPro.TextMeshProUGUI> moneyTexts; 

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SetMode(UIMode.Normal);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetMode(UIMode.Normal);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetMode(UIMode.Inventory);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetMode(UIMode.Shop);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetMode(UIMode.Game);
        foreach(var txt in moneyTexts) { txt.text =$"Money:{gameManager.Money.ToString()}"; }
    }

    public void SetMode(UIMode mode)
    {
        currentMode = mode;

        if (normalUI != null) normalUI.SetActive(mode == UIMode.Normal);
        if (inventoryUI != null) inventoryUI.SetActive(mode == UIMode.Inventory);
        if (shopUI != null) shopUI.SetActive(mode == UIMode.Shop);
        if (gameUI != null) gameUI.SetActive(mode == UIMode.Game);

        if (gameManager != null)
        {
            gameManager.SetMode((GameManager.Mode)mode);
        }
    }

    public UIMode GetCurrentMode()
    {
        return currentMode;
    }
}