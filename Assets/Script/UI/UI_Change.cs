using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Change : MonoBehaviour
{
    public List<Sprite> spriteList;
    public Image uiImage; // 변경할 UI 이미지
    public Sprite variant2Sprite; // X키를 누르면 표시될 이미지

    private Sprite defaultSprite; // 기본 이미지

    private int index = 0;
    void Start()
    {
        // 기본 이미지 설정
        defaultSprite = uiImage.sprite;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            uiImage.sprite = spriteList[index++];
        }
    }
}
