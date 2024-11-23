using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultMode : MonoBehaviour
{   
    public List<Sprite> spriteList = new List<Sprite>();
    public Image image;
    
    private int index = 0;
    
    void Start()
    {
        image = GetComponent<Image>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) index++;
        else if (Input.GetKeyDown(KeyCode.Z)) index--;
        if(index >= spriteList.Count) { index = 0; }
        else if(index < 0) { index = spriteList.Count - 1; }
        ImageChange();
    }
    
    

    private void ImageChange()
    {
        image.sprite = spriteList[index];
    }
}
