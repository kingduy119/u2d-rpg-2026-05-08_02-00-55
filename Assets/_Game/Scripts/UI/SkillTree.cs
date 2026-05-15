using TMPro;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public TMP_Text pointsText;
    public int availablePoints;
    public SkillSlot[] skillSlots;


    private void OnEnable()
    {
        SkillSlot.OnAbilityPointSpent += HandleAbilityPointSpent;
        SkillSlot.OnSkillMaxed += HandleSkillMaxed;
        Level_Manager.OnLevelUp += UpdateAbilityPoints;
    }


    private void OnDisable()
    {
        SkillSlot.OnAbilityPointSpent -= HandleAbilityPointSpent;
        SkillSlot.OnSkillMaxed -= HandleSkillMaxed;
    }

    private void Start()
    {
        foreach (SkillSlot slot in skillSlots)
        {
            slot.skillButton.onClick.AddListener(slot.TryUdpdateLevel);
        }

        UpdatePointsUI();
    }

    private void HandleAbilityPointSpent(SkillSlot slot)
    {
        availablePoints--;
        UpdatePointsUI();
    }

    private void HandleSkillMaxed(SkillSlot skillSlot)
    {
        foreach (SkillSlot slot in skillSlots)
        {
            if (!slot.isUnlocked && slot.CanUnlock())
            {
                slot.UnLock();
            }
        }
    }

    private void UpdateAbilityPoints(int amount)
    {
        availablePoints += amount;
        pointsText.text = "Points: " + availablePoints.ToString();
    }

    private void UpdatePointsUI()
    {
        pointsText.text = "Points: " + availablePoints.ToString();
    }
}
