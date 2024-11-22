using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Change : MonoBehaviour
{   
    private List<Sprite> spriteList = new List<Sprite>();

    // frame1 ~ frame5 스프라이트를 UI assets 폴더에서 로드
    void Awake()
    {
        for (int i = 1; i <= 5; i++)
        {
            Sprite sprite = Resources.Load<Sprite>($"ui assets/UI{i}");
            if (sprite != null)
            {
                spriteList.Add(sprite);
            }
            else
            {
                Debug.LogWarning($"ui assets/Property {i}=Default 스프라이트를 찾을 수 없습니다!");
            }
        }
    }

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
            if (++index >= spriteList.Count) index = 0;
            uiImage.sprite = spriteList[index];
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            if(--index < 0) index = spriteList.Count - 1;
            uiImage.sprite = spriteList[index];
        }
    }
}
