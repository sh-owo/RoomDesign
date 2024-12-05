using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToChoseMap : MonoBehaviour
{
    public void LoadChoseMap()
    {
        UIManager.Instance.SetMode(UIMode.MapManager);
    }
}
