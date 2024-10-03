using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //TODO: 1번이 기본이되는 건설설정(F,G) 2번이 건설한거 움직이는기능(이동,회전)
    //TODO: 물건 배치되는 느낌을 바꿔야함 왜냐하면 지금 배치하면 물건이 튀어다님 그래서 저항 씨게 넣어놨는데 이상함
    public static GameManager Instance { get; private set; }
    public List<GameObject> stuffList = new List<GameObject>();

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

        stuffList.AddRange(Resources.LoadAll<GameObject>("Prefabs"));
        Debug.Log("StuffList Count: " + stuffList.Count);
    }
}