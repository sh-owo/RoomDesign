using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ToAnotherScene : MonoBehaviour
{
    [SerializeField] private List<string> sceneNames;
    
    public void LoadScene()
    {
        int randomIndex = Random.Range(0, sceneNames.Count);
        SceneManager.LoadScene(sceneNames[randomIndex]);
    }
}