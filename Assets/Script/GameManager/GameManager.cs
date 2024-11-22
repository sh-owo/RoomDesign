using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO: 1번이 기본이되는 건설설정(F,G) 2번이 건설한거 움직이는기능(이동,회전)
    //TODO: stuffClass: stuff, 정상적으로 배치되기 위한 pos, 
    public static GameManager Instance { get; private set; }
    public int moveMode = 1; //1: 기본, 2: 물건이동, 3: 물건회전

    public enum StuffMode
    {
        Default = 0,
        Move,
        Rotate
    }

    public StuffMode CurrentMode = StuffMode.Default;
    
    private void Awake()
    {
        Cursor.visible = false;
        Screen.SetResolution(1920, 1080, true);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}