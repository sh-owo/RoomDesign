using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManage : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/Map/office/office");
    }
}
