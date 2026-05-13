using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    [Header("Movement State")]
    public float speed;

    [Header("Combat State")]
    public int damage;
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
            Instance = this;
        else
            Destroy(gameObject);
    }
}
