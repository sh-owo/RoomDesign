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
    public string questName;           
    public string questSeries;         
    public string targetObjectName;    
    public int requiredAmount;
    public int value;
    public bool isCompleted;           
}

public class QuestMaker : MonoBehaviour
{
    [SerializeField]
    private List<Quest> questList = new List<Quest>();

    [SerializeField]
    private float checkInterval = 1f;

    [SerializeField]
    public string currentQuestSeries = "";

    [SerializeField]
    private Slider progressSlider;

    [SerializeField]
    private TextMeshProUGUI progressText;

    [SerializeField]
    private QuestPanelUI questPanel;

    private void Start()
    {
        if (progressSlider != null)
        {
            progressSlider.minValue = 0f;
            progressSlider.maxValue = 100f;
            UpdateProgress();
        }

        StartCoroutine(CheckQuestsRoutine());
    }

    public void AddQuest(string questName, string questSeries, string targetName, int amount)
    {
        Quest newQuest = new Quest
        {
            questName = questName,
            questSeries = questSeries,
            targetObjectName = targetName,
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

    public void SetCurrentQuestSeries(string series)
    {
        currentQuestSeries = series;
        UpdateProgress();
        if (questPanel != null)
        {
            questPanel.DisplayQuestsForCurrentSeries();
        }
    }

    private IEnumerator CheckQuestsRoutine()
    {
        while (true)
        {
            CheckAllQuests();
            UpdateProgress();
            yield return new WaitForSeconds(checkInterval);
        }
    }

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

    private void CheckQuestProgress(Quest quest)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        int count = allObjects.Count(obj => 
            obj.name.StartsWith(quest.targetObjectName) && 
            obj.name.EndsWith("(Clone)") &&
            obj.layer == LayerMask.NameToLayer("Object")  // Tag 대신 Layer 사용
        );
        
        if (count >= quest.requiredAmount)
        {
            CompleteQuest(quest);
        }
    }

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

    private float CalculateSeriesProgress()
    {
        List<Quest> seriesQuests = GetQuestsBySeries(currentQuestSeries);
        
        if (seriesQuests.Count == 0) return 0f;

        int completedQuests = seriesQuests.Count(q => q.isCompleted);
        return (float)completedQuests / seriesQuests.Count * 100f;
    }

    public List<Quest> GetQuestsBySeries(string series)
    {
        if (string.IsNullOrEmpty(series))
            return questList;
        return questList.FindAll(q => q.questSeries == series);
    }

    public int GetCurrentAmount(string targetName)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        return allObjects.Count(obj => 
            obj.name.StartsWith(targetName) && 
            obj.name.EndsWith("(Clone)") &&
            obj.layer == LayerMask.NameToLayer("Object")  // Tag 대신 Layer 사용
        );
    }
    
    public int GetPercent() => (int)progressSlider.value;
    
    public int GetReward()
    {
        List<Quest> seriesQuests = GetQuestsBySeries(currentQuestSeries);
    
        if (seriesQuests.Count == 0) return 0;

        // 현재 시리즈의 완료된 퀘스트들의 value 합계 계산
        int totalReward = seriesQuests
            .Where(q => q.isCompleted)
            .Sum(q => q.value);

        return totalReward;
    }
}