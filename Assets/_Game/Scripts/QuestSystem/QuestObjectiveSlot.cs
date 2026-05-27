using TMPro;
using UnityEngine;

public class QuestObjectiveSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text txtObjective;
    [SerializeField] private TMP_Text txtTracking;


    public void RefreshObjectives(string description, string progressText, bool isComplete)
    {
        txtObjective.text = description;
        txtTracking.text = progressText;

        Color color = isComplete ? Color.gray : Color.black;
        txtObjective.color = color;
        txtTracking.color = color;
    }
}
