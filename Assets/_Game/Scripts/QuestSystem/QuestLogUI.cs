using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class QuestLogUI : MonoBehaviour
{
    [SerializeField] private TMP_Text questName;
    [SerializeField] private TMP_Text questDescription;
    [SerializeField] private QuestObjectiveSlot[] objectiveSlots;
    [SerializeField] private QuestRewardSlot[] rewardSlots;

    [Header("Quest Buttons")]
    [SerializeField] private CanvasGroup cvgAccept;
    [SerializeField] private CanvasGroup cvgDecline;
    [SerializeField] private CanvasGroup cvgComplete;

    public QuestLogSlot[] questSlots;

    private QuestSO questSO;
    public QuestSO noAvailableQuest;

    private void OnEnable()
    {
        QuestEvents.OnQuestOfferRequested += ShowQuestOffer;
    }

    private void OnDisable()
    {
        QuestEvents.OnQuestOfferRequested -= ShowQuestOffer;
    }

    private void Start()
    {
        RefreshQuestList();
    }

    public void ShowQuestOffer(QuestSO incomingQuestSO)
    {
        if (QuestManager.Instance.IsQuestAccepted(incomingQuestSO))
        {
            questSO = noAvailableQuest;
            SetCanvasState(cvgAccept, false);
            SetCanvasState(cvgDecline, true);
            SetCanvasState(cvgComplete, false);
        }
        else
        {
            questSO = incomingQuestSO;
            SetCanvasState(cvgAccept, true);
            SetCanvasState(cvgDecline, true);
            SetCanvasState(cvgComplete, false);

        }
        HandleQuestClicked(questSO);
    }


    public void OnAcceptQuestClicked()
    {
        QuestManager.Instance.AcceptQuest(questSO);
        QuestManager.Instance.CloseCanvas();
        SetCanvasState(cvgAccept, false);
        SetCanvasState(cvgComplete, false);
        RefreshQuestList();
    }

    public void OnDeclineQuestClicked()
    {
        QuestManager.Instance.CloseCanvas();
    }

    public void OnCompleteQuestClicked()
    {
        QuestManager.Instance.CompleteQuest(questSO);
        QuestManager.Instance.CloseCanvas();
        RefreshQuestList();
    }

    public void RefreshQuestList()
    {
        List<QuestSO> activeQuests = QuestManager.Instance.GetActiveQuests();
        activeQuests.Sort((a, b) => a.questLevel.CompareTo(b.questLevel));

        for (int i = 0; i < questSlots.Length; i++)
        {
            if (i < activeQuests.Count)
            {
                questSlots[i].SetQuest(activeQuests[i]);
                questSlots[i].gameObject.SetActive(true);
            }
            else
            {
                questSlots[i].ClearSlot();
            }
        }
    }

    private void SetCanvasState(CanvasGroup group, bool active)
    {
        group.alpha = active ? 1 : 0;
        group.blocksRaycasts = active;
        group.interactable = active;
    }

    public void HandleQuestClicked(QuestSO questSO)
    {
        this.questSO = questSO;
        questName.text = questSO.questName;
        questDescription.text = questSO.questDescription;

        DisplayObjectives();
        DisplayReward();

        bool isComplete = QuestManager.Instance.IsQuestComplete(questSO);
        if (isComplete)
        {
            SetCanvasState(cvgAccept, false);
            SetCanvasState(cvgDecline, false);
            SetCanvasState(cvgComplete, true);
        }


        foreach (var objective in questSO.objectives)
        {
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
