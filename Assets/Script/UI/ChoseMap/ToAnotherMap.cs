using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToAnotherMap : MonoBehaviour
{
    [SerializeField] private int mapindex;
    [SerializeField] private QuestMaker questMaker;
    
    
    public void LoadScene()
    {
        UIManager.Instance.SetMode(UIMode.Normal);
        switch (mapindex)
        {
            case 0: SceneManager.LoadScene("Scenes/Map/Theme1/basic"); questMaker.currentQuestSeries = "one"; break;
            case 1: SceneManager.LoadScene("Scenes/Map/Theme2/christmas"); questMaker.currentQuestSeries = "two";break;
            case 2: SceneManager.LoadScene("Scenes/Map/Theme3/halloween"); questMaker.currentQuestSeries = "three";break;
        }
    }
}
