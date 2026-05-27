using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    [Header("UI References")]
    public CanvasGroup dialogueCanvasGroup;
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] optionButtons;

    public bool isDialogueActive;

    private DialogueSO currentDialogue;
    private int dialogueIndex = 0;
    private float lastDialogueEndTime;
    private float dialogueCooldown = .1f;

    void Awake()
    {
        CloseCanvas();
        ClearOptions();
    }


    public void StartDialogue(DialogueSO dialogueSO)
    {
        // if (Time.unscaledTime - lastDialogueEndTime < dialogueCooldown)
        //     return;

        dialogueIndex = 0;
        currentDialogue = dialogueSO;
        isDialogueActive = true;
        ShowDialogure();
    }

    public bool CanStartDialogue()
    {
        return Time.unscaledTime - lastDialogueEndTime >= dialogueCooldown;
    }

    public void AdvanceDialogue()
    {
        if (dialogueIndex < currentDialogue.lines.Length)
            ShowDialogure();
        else
            EndOrShowOptions();
    }

    private void ShowDialogure()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];
        GameManager.Instance.dialogueHistoryTracker.RecordNPC(line.speaker);

        portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;

        dialogueText.text = line.text;
        dialogueIndex++;

        OpenCanvas();
    }

    private void EndOrShowOptions()
    {
        if (currentDialogue.options.Length > 0)
        {
            for (int i = 0; i < currentDialogue.options.Length; i++)
            {
                var option = currentDialogue.options[i];
                optionButtons[i].GetComponentInChildren<TMP_Text>().text = option.optionText;
                optionButtons[i].gameObject.SetActive(true);

                optionButtons[i].onClick.AddListener(() => ChoiceOption(option.nextDialogue));
            }
        }
        else
        {
            EndDialogue();
        }

        EventSystem.current.SetSelectedGameObject(optionButtons[0].gameObject);
    }

    private void ChoiceOption(DialogueSO dialogueSO)
    {
        if (dialogueSO == null)
            EndDialogue();
        else
        {
            StartDialogue(dialogueSO);
            ClearOptions();
        }
    }

    private void ClearOptions()
    {
        foreach (var option in optionButtons)
        {
            option.gameObject.SetActive(false);
            option.onClick.RemoveAllListeners();
        }
    }

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        CloseCanvas();

        lastDialogueEndTime = Time.unscaledTime;
    }


    private void OpenCanvas()
    {
        dialogueCanvasGroup.alpha = 1f;
        dialogueCanvasGroup.interactable = true;
        dialogueCanvasGroup.blocksRaycasts = true;
    }
    private void CloseCanvas()
    {
        dialogueCanvasGroup.alpha = 0f;
        dialogueCanvasGroup.interactable = false;
        dialogueCanvasGroup.blocksRaycasts = false;
    }


}
