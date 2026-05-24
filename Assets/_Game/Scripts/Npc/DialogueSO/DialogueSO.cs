using System;
using UnityEngine;
[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueNode")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueOption[] options;

    [Header("Conditional Requirements (Optional)")]
    public ActorSO[] requritedNPCS;


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

        // TODO:

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