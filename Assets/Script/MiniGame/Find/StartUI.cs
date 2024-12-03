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
            //TODO: 씬 넣어놓기
            // SceneManager.LoadScene("TODO");
            
        }
        else if(FindGameManager.Instance.isGameEnd && !FindGameManager.Instance.isPlayerWon)
        {
            startText.text = "You Lose!";
            StartCoroutine(waitSecond());
            //TODO: 씬 넣어놓기
            // SceneManager.LoadScene("TODO");
        }
    }

    IEnumerator waitSecond()
    {
        yield return new WaitForSeconds(10);
    }
}
