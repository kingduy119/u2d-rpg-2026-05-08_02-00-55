using UnityEngine;

public class EnemyCombat : MonoBehaviour
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
            collision.gameObject.GetComponent<PlayerHealth>().ChangeHealth(-damage);
        }
    }

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, playerLayer);
        foreach (Collider2D player in hits)
        {
            player.GetComponent<PlayerHealth>().ChangeHealth(-damage);
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
