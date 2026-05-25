using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Conditional Requirements (Optional)")]
    public ActorSO[] requritedNPCS;
    public LocationSO[] requiredLocations;
    public ItemSO[] requiredItems;


    public bool IsConditionMet()
    {
        if (requritedNPCS.Length > 0)
        {
            foreach (var npc in requritedNPCS)
            {
                if (!DialogueHistoryTracker.Instance.HasSpokenWith(npc))
                    return false;
            }
        }

        if (requiredLocations.Length > 0)
        {
            foreach (var location in requiredLocations)
            {
                if (!LocationHistoryTracker.Instance.HasVisited(location))
                    return false;
            }
        }

        if (requiredItems.Length > 0)
        {
            foreach (var item in requiredItems)
            {
                if (!InventoryManager.Instance.HasItem(item))
                    return false;
            }
        }

        return true;
    }
}

[Serializable]
public class DialogueLine
{
    public ActorSO speaker;
    [TextArea(3, 5)] public string text;
}


[Serializable]
public class DialogueOption
{
    public string optionText;
    public DialogueSO nextDialogue;
}