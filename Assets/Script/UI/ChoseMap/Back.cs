using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    public void BackToSceneMove()
    {
        UIManager.Instance.SetMode(UIMode.SceneMove);
    }
}
