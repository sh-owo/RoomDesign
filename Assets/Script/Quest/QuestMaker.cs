using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Quest
{
    public string questName;           // 퀘스트 이름
    public string questSeries;         // 퀘스트 시리즈
    public string targetObjectTag;     // 찾을 오브젝트의 태그
    public int requiredAmount;         // 필요한 오브젝트 수량
    public bool isCompleted;           // 퀘스트 완료 여부
}

public class QuestMaker : MonoBehaviour
{
    [SerializeField]
    private List<Quest> questList = new List<Quest>();  // 퀘스트 목록

    [SerializeField]
    private float checkInterval = 1f;  // 퀘스트 체크 주기

    [SerializeField]
    public string currentQuestSeries = "";  // 현재 활성화된 퀘스트 시리즈

    [SerializeField]
    private Slider progressSlider;     // 진행률을 표시할 슬라이더

    [SerializeField]
    private TextMeshProUGUI progressText;   // 진행률을 텍스트로 표시

    [SerializeField]
    private QuestPanelUI questPanel;   // QuestPanelUI 참조

    private void Start()
    {
        // 슬라이더 초기화
        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 100f;
            UpdateProgress();
        }

        // 주기적으로 퀘스트 상태를 체크하는 코루틴 시작
        StartCoroutine(CheckQuestsRoutine());
    }

    // 퀘스트 추가 메소드
    public void AddQuest(string questName, string questSeries, string targetTag, int amount)
    {
        Quest newQuest = new Quest
        {
            questName = questName,
            questSeries = questSeries,
            targetObjectTag = targetTag,
            requiredAmount = amount,
            isCompleted = false
        };
        questList.Add(newQuest);
        UpdateProgress();
        if (questPanel != null)
        {
            questPanel.DisplayQuestsForCurrentSeries();
        }
    }

    // 현재 퀘스트 시리즈 설정
    public void SetCurrentQuestSeries(string series)
    {
        currentQuestSeries = series;
        UpdateProgress();
        if (questPanel != null)
        {
            questPanel.DisplayQuestsForCurrentSeries();
        }
    }

    // 퀘스트 체크 코루틴
    private IEnumerator CheckQuestsRoutine()
    {
        while (true)
        {
            CheckAllQuests();
            UpdateProgress();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    // 모든 퀘스트의 상태를 체크
    private void CheckAllQuests()
    {
        foreach (Quest quest in questList)
        {
            if (!quest.isCompleted && 
                (string.IsNullOrEmpty(currentQuestSeries) || quest.questSeries == currentQuestSeries))
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
        Debug.Log($"퀘스트 '{quest.questName}'가 완료되었습니다!");
        UpdateProgress();
        if (questPanel != null)
        {
            questPanel.DisplayQuestsForCurrentSeries();
        }
    }

    // 현재 시리즈의 진행률 업데이트
    private void UpdateProgress()
    {
        if (progressSlider == null) return;

        float progress = CalculateSeriesProgress();
        progressSlider.value = progress;

        if (progressText != null)
        {
            progressText.text = $"{progress:F1}%";
        }
    }

    // 현재 시리즈의 진행률 계산
    private float CalculateSeriesProgress()
    {
        List<Quest> seriesQuests = GetQuestsBySeries(currentQuestSeries);
        
        if (seriesQuests.Count == 0) return 0f;

        int completedQuests = seriesQuests.Count(q => q.isCompleted);
        return (float)completedQuests / seriesQuests.Count * 100f;
    }

    // 특정 시리즈의 모든 퀘스트 조회
    public List<Quest> GetQuestsBySeries(string series)
    {
        if (string.IsNullOrEmpty(series))
            return questList;
        return questList.FindAll(q => q.questSeries == series);
    }

    // 퀘스트 진행상황 확인용 메소드
    public int GetCurrentAmount(string targetTag)
    {
        return GameObject.FindGameObjectsWithTag(targetTag).Length;
    }
}