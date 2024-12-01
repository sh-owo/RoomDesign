using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;           // 퀘스트 이름
    public string targetObjectTag;     // 찾을 오브젝트의 태그
    public int requiredAmount;         // 필요한 오브젝트 수량
    public bool isCompleted;           // 퀘스트 완료 여부
    public UnityEngine.Events.UnityEvent onQuestComplete;  // 퀘스트 완료시 실행될 이벤트
}

public class QuestMaker : MonoBehaviour
{
    [SerializeField]
    private List<Quest> questList = new List<Quest>();  // 퀘스트 목록

    [SerializeField]
    private float checkInterval = 1f;  // 퀘스트 체크 주기

    private void Start()
    {
        // 주기적으로 퀘스트 상태를 체크하는 코루틴 시작
        StartCoroutine(CheckQuestsRoutine());
    }

    // 퀘스트 추가 메소드
    public void AddQuest(string questName, string targetTag, int amount)
    {
        Quest newQuest = new Quest
        {
            questName = questName,
            targetObjectTag = targetTag,
            requiredAmount = amount,
            isCompleted = false
        };
        questList.Add(newQuest);
    }

    // 퀘스트 체크 코루틴
    private IEnumerator CheckQuestsRoutine()
    {
        while (true)
        {
            CheckAllQuests();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    // 모든 퀘스트의 상태를 체크
    private void CheckAllQuests()
    {
        foreach (Quest quest in questList)
        {
            if (!quest.isCompleted)
            {
                CheckQuestProgress(quest);
            }
        }
    }

    // 개별 퀘스트 진행상황 체크
    private void CheckQuestProgress(Quest quest)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(quest.targetObjectTag);
        
        if (objects.Length >= quest.requiredAmount)
        {
            CompleteQuest(quest);
        }
    }

    // 퀘스트 완료 처리
    private void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        quest.onQuestComplete?.Invoke();
        Debug.Log($"퀘스트 '{quest.questName}'가 완료되었습니다!");
    }

    // 퀘스트 진행상황 확인용 메소드
    public int GetCurrentAmount(string targetTag)
    {
        return GameObject.FindGameObjectsWithTag(targetTag).Length;
    }
}