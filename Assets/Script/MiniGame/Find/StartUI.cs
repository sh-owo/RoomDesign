using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI startText;

    public void LoadUI(int second)
    {
        startText.text = $"Game starts in {second}...";
    }

    public void Start()
    {
        startText.text = "";
    }

    void Update()
    {
        if(FindGameManager.Instance.isGameEnd && FindGameManager.Instance.isPlayerWon)
        {
            startText.text = $"You Win!(+{FindGameManager.Instance.prize})";
            StartCoroutine(waitSecond());
        }
        else if(FindGameManager.Instance.isGameEnd && !FindGameManager.Instance.isPlayerWon)
        {
            startText.text = "You Lose!";
            StartCoroutine(waitSecond());
            
        }
    }

    IEnumerator waitSecond()
    {
        yield return new WaitForSeconds(2);
        UIManager.Instance.SetMode(UIMode.Normal);
        SceneManager.LoadScene("Scenes/Map/office/office");
    }
}
