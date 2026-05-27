using UnityEngine;

public class QuestLogUI : MonoBehaviour
{
    public void HandleQuestClicked(QuestSO questSO)
    {
        Debug.Log($"Clicked Quest: {questSO.questName}");
        foreach (var objective in questSO.objectives)
        {
            QuestManager.Instance.UpdateObjectiveProgress(questSO, objective);
            Debug.Log($"Objective: {objective.description} => {QuestManager.Instance.GetProgressText(questSO, objective)}");
        }
    }
}
