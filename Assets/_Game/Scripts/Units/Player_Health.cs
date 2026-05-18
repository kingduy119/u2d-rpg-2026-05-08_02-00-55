using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Health : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public Slider healthSlider;
    public TMP_Text healthText;

    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;
        healthText.text = $"{currentHealth:0.0} / {maxHealth}";
    }

    public void ChangeHealth(float amount)
    {
        currentHealth += amount;
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
        healthText.text = $"{currentHealth} / {maxHealth}";
    }
}
