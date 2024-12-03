using UnityEngine;
using TMPro;

public class QuestPanelUI : MonoBehaviour
{
    [SerializeField]
    private Transform panel;               // 퀘스트 목록이 표시될 Panel
    [SerializeField]
    private GameObject questSummaryPrefab; // Quest Summary 프리팹
    [SerializeField]
    private QuestMaker questMaker;         // QuestMaker 참조

    private void OnEnable()
    {
        // 패널이 활성화될 때 처음 표시
        DisplayQuestsForCurrentSeries();
    }

    public void DisplayQuestsForCurrentSeries()
    {
        if (questMaker == null || panel == null) return;

        // 기존 패널의 모든 자식 오브젝트 제거
        foreach (Transform child in panel)
        {
            Destroy(child.gameObject);
        }

        // QuestMaker에서 현재 시리즈의 퀘스트들을 가져와서 표시
        var questList = questMaker.GetQuestsBySeries(questMaker.currentQuestSeries);

        foreach (var quest in questList)
        {
            // Quest Summary 프리팹 생성
            GameObject summaryObj = Instantiate(questSummaryPrefab, panel);
            
            // QuestSummary의 TMP Text 컴포넌트를 찾아서 퀘스트 이름 설정
            Transform questSummaryTransform = summaryObj.transform.Find("QuestSummary");
            if (questSummaryTransform != null)
            {
                TextMeshProUGUI questNameText = questSummaryTransform.GetComponent<TextMeshProUGUI>();
                if (questNameText != null)
                {
                    questNameText.text = quest.questName;
                }
            }

            // Panel 하위의 V 오브젝트를 찾아서 완료 상태에 따라 활성화/비활성화
            Transform panelTransform = summaryObj.transform.Find("Panel");
            if (panelTransform != null)
            {
                Transform checkMark = panelTransform.Find("V");
                if (checkMark != null)
                {
                    checkMark.gameObject.SetActive(quest.isCompleted);
                }
            }
        }
    }
}