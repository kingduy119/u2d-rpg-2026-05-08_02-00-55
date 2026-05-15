using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateUI : MonoBehaviour
{
    public GameObject panel;
    public GameObject[] states;

    void Start()
    {
        UpdateDamage();
        panel.SetActive(false);
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     Toggle();
        // }
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            Toggle();
        }
    }

    void Toggle()
    {
        panel.SetActive(!panel.activeSelf);
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
