using UnityEngine;

public class Enemy_Combat : MonoBehaviour
{
    public float damage = 0.2f;
    public float weaponRange;
    public float knockForce = 1f;
    public float stunTime = 1f;
    public Transform attackPoint;
    public LayerMask playerLayer;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StateManager.Instance.UpdateHealth(-damage);
        }
    }

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        foreach (Collider2D player in hits)
        {
            StateManager.Instance.UpdateHealth(-damage);
            player.GetComponent<Player>().KnockBack(transform, knockForce, stunTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, weaponRange);
    }
}
