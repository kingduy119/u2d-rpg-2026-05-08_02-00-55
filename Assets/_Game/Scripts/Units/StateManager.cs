using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    [Header("Movement State")]
    public float speed;

    [Header("Combat State")]
    public int damage;
    public float attackSpeed = 1.5f;

    public float weaponRange;
    public float knockbackForce;
    public float knockbackTime;
    public float stunTime;

    [Header("Health State")]
    public float maxHealth;
    public float currentHealth;

    public Slider healthSlider;
    public TMP_Text healthText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void UpdateMaxHealth(float amount)
    {
        maxHealth += amount;
        UpdateHealthUI();
    }

    public void UpdateHealth(float amount)
    {
        currentHealth += amount;
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        healthSlider.value = currentHealth;
        healthSlider.maxValue = maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }
}
