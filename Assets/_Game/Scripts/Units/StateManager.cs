using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    [Header("Movement State")]
    public float speed { get; set; }

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

    public void UpdateMaxHealth(int amount)
    {
        maxHealth += amount;
    }

    public void UpdateHealth(int amount)
    {
        currentHealth += amount;
    }

}
