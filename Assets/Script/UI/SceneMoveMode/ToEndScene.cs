using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToEndScene : MonoBehaviour
{
    [SerializeField] private int NeedMoney = 10000;
    private int money = GameManager.Instance.Money;
    public void LoadEndScene()
    {
        if(money >= NeedMoney)
        {
            //TODO: 엔딩씬 연결
            SceneManager.LoadScene("temp");
        }
        else
        {
            Debug.Log("need more money");
        }
    }
}
