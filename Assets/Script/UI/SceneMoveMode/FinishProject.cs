using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class FinishProject : MonoBehaviour
{
    public QuestMaker questMaker;
    public Button button;

    void Update()
    {
        if(questMaker.GetPercent() == 100)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
    public void Finish()
    {
        int percent = questMaker.GetPercent();
        int reward = questMaker.GetReward();
        if (percent == 100)
        {
            GameManager.Instance.Money += reward; 
            SceneManager.LoadScene("Scenes/Map/office/office");
        }
    }
}
