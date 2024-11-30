using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToChangeScene : MonoBehaviour
{
    public void Move()
    {
        GameManager.Instance.SetMode(GameManager.Mode.MoveManager);
    }
}
