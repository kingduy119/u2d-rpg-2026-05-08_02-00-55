using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public CanvasGroup dialogueCanvasGroup;
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;

    public bool isDialogueActive;

    private DialogueSO currentDialogue;
    private int dialogueIndex = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        CloseCanvas();
    }

    void Start()
    {
        ShowDialogure();
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        ShowDialogure();
    }

    public void AdvanceDialogue()
    {
        if (dialogueIndex < currentDialogue.lines.Length)
            ShowDialogure();
        else
            EndDialogue();
    }

    private void ShowDialogure()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;

        dialogueText.text = line.text;
        dialogueIndex++;

        OpenCanvas();
    }

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        CloseCanvas();
    }

    private void CloseCanvas()
    {
        dialogueCanvasGroup.alpha = 0f;
        dialogueCanvasGroup.interactable = false;
        dialogueCanvasGroup.blocksRaycasts = false;
    }

    private void OpenCanvas()
    {
        dialogueCanvasGroup.alpha = 1f;
        dialogueCanvasGroup.interactable = true;
        dialogueCanvasGroup.blocksRaycasts = true;
    }
}
