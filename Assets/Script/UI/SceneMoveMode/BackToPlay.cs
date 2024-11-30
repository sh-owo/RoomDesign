using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToPlay : MonoBehaviour
{
    public void Back()
    {
        GameManager.Instance.SetMode(GameManager.Mode.Normal);
    }
}
