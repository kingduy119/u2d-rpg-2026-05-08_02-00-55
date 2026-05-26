using TMPro;
using UnityEngine;

public class QuestLogSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text questName;
    [SerializeField] private TMP_Text questLevel;

    public QuestSO currentQuest;
    public QuestLogUI questLogUI;

    void OnValidate()
    {
        if (currentQuest != null)
            SetQuest(currentQuest);
        else
            gameObject.SetActive(false);
    }

    public void SetQuest(QuestSO questSO)
    {
        currentQuest = questSO;
        questName.text = questSO.questName;
        questLevel.text = "Lv." + questSO.questLevel;
    }

    public void OnSlotClicked()
    {
        questLogUI.HandleQuestClicked(currentQuest);
    }
}
