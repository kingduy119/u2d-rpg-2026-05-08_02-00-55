using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    private Dictionary<QuestSO, Dictionary<QuestObjective, int>> questProgress = new();
    private CanvasGroup canvasGroup;
    private bool isOpenCanvas = false;

    public QuestSO[] currentQuests;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var questSO in currentQuests)
        {
            AcceptQuest(questSO);
        }
    }

    private void OnEnable()
    {
        QuestEvents.IsQuestCompelete += IsQuestComplete;
    }
    private void OnDisable()
    {
        QuestEvents.IsQuestCompelete -= IsQuestComplete;
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

    public bool IsQuestAccepted(QuestSO questSO)
    {
        return questProgress.ContainsKey(questSO);
    }

    public bool IsQuestComplete(QuestSO questSO)
    {
        if (!questProgress.TryGetValue(questSO, out var progressDict))
            return false;

        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
        foreach (var objective in questSO.objectives)
        {
            if (progressDict[objective] < objective.requiredAmount)
                return false;
        }

        return true;
    }

    public List<QuestSO> GetActiveQuests()
    {
        return new List<QuestSO>(questProgress.Keys);
    }

    public void AcceptQuest(QuestSO questSO)
    {
        questProgress[questSO] = new Dictionary<QuestObjective, int>();
        foreach (var objective in questSO.objectives)
        {
            UpdateObjectiveProgress(questSO, objective);
        }
        Debug.Log($"questProgress: {questProgress.Count}");
    }

    public void CompleteQuest(QuestSO questSO)
    {
        questProgress.Remove(questSO);
        foreach (var reward in questSO.rewards)
        {
            InventoryManager.Instance.AddItem(reward.itemSO, reward.quantity);
        }
    }

    public void UpdateObjectiveProgress(QuestSO questSO, QuestObjective questObjective)
    {
        if (!questProgress.ContainsKey(questSO))
            return;
        // questProgress[questSO] = new Dictionary<QuestObjective, int>();

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

    public void OpenCanvas()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isOpenCanvas = true;
    }
    public void CloseCanvas()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isOpenCanvas = false;
    }
}
