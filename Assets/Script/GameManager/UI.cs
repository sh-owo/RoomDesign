using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }
    private void Awake()
    {
        // 이미 인스턴스가 있는지 확인
        if (Instance != null && Instance != this)
        {
            // 이미 존재하는 경우 현재 오브젝트 삭제
            Destroy(gameObject);
            return;
        }

        // 처음 생성된 인스턴스 설정
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
