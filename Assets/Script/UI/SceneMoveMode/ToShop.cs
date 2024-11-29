using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToShop : MonoBehaviour
{
    public void LoadShop()
    {
        GameManager.Instance.SetMode(GameManager.Mode.Shop);
    }
}
