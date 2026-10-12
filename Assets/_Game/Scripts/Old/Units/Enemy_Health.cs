using UnityEngine;

public class Enemy_Health : MonoBehaviour
{
    public int expReward = 50;
    public delegate void MonsterDeath(int exp);
    public static event MonsterDeath OnMonsterDeath;

    public int maxHealth = 10;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else if (currentHealth <= 0)
        {
            OnMonsterDeath(expReward);
            Destroy(gameObject);
        }
    }
}
