using TMPro;
using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questName;
    [SerializeField] private TMP_Text questDescription;
    [SerializeField] private QuestObjectiveSlot[] objectiveSlots;
    [SerializeField] private QuestRewardSlot[] rewardSlots;

    private QuestSO questSO;

    public void HandleQuestClicked(QuestSO questSO)
    {
        this.questSO = questSO;
        questName.text = questSO.questName;
        questDescription.text = questSO.questDescription;

        DisplayObjectives();
        DisplayReward();

        foreach (var objective in questSO.objectives)
        {
            // QuestManager.Instance.UpdateObjectiveProgress(questSO, objective);
            Debug.Log($"Objective: {objective.description} => {QuestManager.Instance.GetProgressText(questSO, objective)}");
        }
    }

    private void DisplayObjectives()
    {
        for (int i = 0; i < objectiveSlots.Length; i++)
        {
            if (i < questSO.objectives.Count)
            {
                var objective = questSO.objectives[i];
                QuestManager.Instance.UpdateObjectiveProgress(questSO, objective);

                int currentAmount = QuestManager.Instance.GetCurrentAmount(questSO, objective);
                string progress = QuestManager.Instance.GetProgressText(questSO, objective);
                bool isComplete = currentAmount >= objective.requiredAmount;

                objectiveSlots[i].gameObject.SetActive(true);
                objectiveSlots[i].RefreshObjectives(objective.description, progress, isComplete);
            }
            else
            {
                objectiveSlots[i].gameObject.SetActive(false);
            }
        }
    }

    private void DisplayReward()
    {
        for (int i = 0; i < rewardSlots.Length; i++)
        {
            if (i < questSO.rewards.Count)
            {
                var reward = questSO.rewards[i];
                rewardSlots[i].DisplayReward(reward.itemSO.itemIcon, reward.quantity);
                rewardSlots[i].gameObject.SetActive(true);
            }
            else
            {
                rewardSlots[i].gameObject.SetActive(false);
            }
        }
    }
}
