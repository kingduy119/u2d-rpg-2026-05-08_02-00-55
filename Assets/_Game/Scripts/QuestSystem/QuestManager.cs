using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();
    private CanvasGroup canvasGroup;
    private bool isOpenCanvas = false;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        CloseCanvas();
    }

    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (isOpenCanvas)
                CloseCanvas();
            else
                OpenCanvas();
        }
    }

    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective questObjective)
    {
        if (!questProgress.ContainsKey(questSO))
            questProgress[questSO] = new Dictionary<QuestObjective, int>();

        var progressDictionary = questProgress[questSO];
        int newAmount = 0;

        if (questObjective.targetItem != null)
            newAmount = InventoryManager.Instance.GetItemQuantity(questObjective.targetItem);
        else if (questObjective.targetLocation != null && GameManager.Instance.locationHistoryTracker.HasVisited(questObjective.targetLocation))
            newAmount = questObjective.requiredAmount;
        else if (questObjective.targetNPC != null && GameManager.Instance.dialogueHistoryTracker.HasSpokenWith(questObjective.targetNPC))
            newAmount = questObjective.requiredAmount;

        progressDictionary[questObjective] = newAmount;
    }

    public string GetProgressText(QuestSO questSO, QuestObjective questObjective)
    {
        int currentAmount = GetCurrentAmount(questSO, questObjective);
        if (currentAmount >= questObjective.requiredAmount)
            return "Complete";
        else if (questObjective.targetItem != null)
            return $"{currentAmount}/{questObjective.requiredAmount}";

        return "In Progress";
    }

    public int GetCurrentAmount(QuestSO questSO, QuestObjective questObjective)
    {
        if (questProgress.TryGetValue(questSO, out var objectiveDictionary))
            if (objectiveDictionary.TryGetValue(questObjective, out int amount))
                return amount;
        return 0;
    }

    private void OpenCanvas()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isOpenCanvas = true;
    }
    private void CloseCanvas()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isOpenCanvas = false;
    }
}
