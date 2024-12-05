using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToEndScene : MonoBehaviour
{
    [SerializeField] private int NeedMoney = 10000;
    public Button button;

    void Update()
    {
        int money = GameManager.Instance.Money;
        button.interactable = (money <= NeedMoney);
    }

    public void LoadEndScene()
    {
        int money = GameManager.Instance.Money;
        if(money >= NeedMoney)
        {
            SceneManager.LoadScene("Scenes/ending/ending");
        }
        else
        {
            Debug.Log("need more money");
        }
    }
}
