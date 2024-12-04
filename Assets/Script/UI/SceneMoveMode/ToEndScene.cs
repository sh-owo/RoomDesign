using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToEndScene : MonoBehaviour
{
    [SerializeField] private int NeedMoney = 10000;
    
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
