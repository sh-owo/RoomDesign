using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ending_change : MonoBehaviour
{
    public Image displayImage;       // UI Image 컴포넌트
    public int endingCount = 4;      // 엔딩 이미지 개수
    public float displayTime = 3f;  // 각 이미지가 표시되는 시간

    private void Start()
    {
        if (displayImage == null)
        {
            Debug.LogError("Display Image is not assigned!");
            return;
        }

        StartCoroutine(DisplayEnding());
    }

    private IEnumerator DisplayEnding()
    {
        for (int i = 1; i <= endingCount; i++)
        {
            // Resources 폴더에서 엔딩 이미지를 로드
            Sprite nextSprite = Resources.Load<Sprite>($"EndingImages/엔딩 {i}");

            if (nextSprite != null)
            {
                displayImage.sprite = nextSprite; // 이미지 전환
                yield return new WaitForSeconds(displayTime); // 3초 대기
            }
            else
            {
                Debug.LogWarning($"Image '엔딩 {i}' not found in Resources/EndingImages!");
            }
        }

        Debug.Log("Ending sequence completed!");
    }
}
