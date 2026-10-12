using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public SkillSO skillSO;
    public Image skillIcon;
    public int currentLevel;
    public bool isUnlocked;

    public TMP_Text levelText;
    public Button skillButton;
    public List<SkillSlot> prerequisitesSkillSlots;

    public static event Action<SkillSlot> OnAbilityPointSpent;
    public static event Action<SkillSlot> OnSkillMaxed;

    void Start()
    {
        UpdateUI();
    }

    private void OnValidate()
    {
        if (skillSO != null && levelText != null)
        {
            skillIcon.sprite = skillSO.skillIcon;
        }
    }

    public void TryUdpdateLevel()
    {

        if (isUnlocked && currentLevel < skillSO.maxLevel)
        {
            currentLevel++;
            OnAbilityPointSpent?.Invoke(this);
            UpdateUI();
        }

        if (currentLevel >= skillSO.maxLevel)
        {
            OnSkillMaxed?.Invoke(this);
        }
    }

    public bool CanUnlock()
    {
        foreach (SkillSlot slot in prerequisitesSkillSlots)
        {
            if (!slot.isUnlocked || slot.currentLevel < slot.skillSO.maxLevel)
            {
                return false;
            }
        }
        return true;
    }

    public void UnLock()
    {
        isUnlocked = true;
        skillIcon.color = Color.white;
        UpdateUI();
    }

    private void UpdateUI()
    {
        skillIcon.sprite = skillSO.skillIcon;
        if (isUnlocked)
        {
            levelText.text = currentLevel.ToString()
                + "/" + skillSO.maxLevel.ToString();
            levelText.fontSize = 12;
        }
        else
        {
            levelText.text = "Locked";
            levelText.fontSize = 6;
            skillIcon.color = Color.gray;
        }
    }
}
