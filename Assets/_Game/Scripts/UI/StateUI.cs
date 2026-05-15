using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateUI : MonoBehaviour
{
    public GameObject statePanel;
    public GameObject skillTreePanel;
    public GameObject[] states;

    void Start()
    {
        UpdateDamage();
        statePanel.SetActive(false);
        skillTreePanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            ToggleStatePanel();
        }
        else if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            ToggleSkillTreePanel();
        }
    }

    void ToggleStatePanel()
    {
        statePanel.SetActive(!statePanel.activeSelf);
    }

    void ToggleSkillTreePanel()
    {
        skillTreePanel.SetActive(!skillTreePanel.activeSelf);
    }


    public void UpdateDamage()
    {
        states[0].GetComponentInChildren<TMP_Text>().text = "Move Speed: " + StateManager.Instance.speed.ToString();
        states[1].GetComponentInChildren<TMP_Text>().text = "Damage: " + StateManager.Instance.damage.ToString();
        states[2].GetComponentInChildren<TMP_Text>().text = "weaponRange: " + StateManager.Instance.weaponRange.ToString();
        states[3].GetComponentInChildren<TMP_Text>().text = "maxHealth: " + StateManager.Instance.maxHealth.ToString();
        states[4].GetComponentInChildren<TMP_Text>().text = "attackSpeed: " + StateManager.Instance.attackSpeed.ToString();
    }
}
