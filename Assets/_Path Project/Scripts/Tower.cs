using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData data;
    private CircleCollider2D _circleCollider;
    public List<Point> _enemiesInRange = new List<Point>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.radius = data.range;
        // _circleCollider.isTrigger = true;

        _enemiesInRange = new List<Point>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            Point enemy = collision.GetComponent<Point>();
            if (enemy != null)
            {
                Debug.Log("Enemy entered range: " + enemy.name);
                _enemiesInRange.Add(enemy);
                // Optionally, start shooting at the enemy here
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Point enemy = collision.GetComponent<Point>();
            if (enemy != null)
            {
                Debug.Log("Enemy exited range: " + enemy.name);
                _enemiesInRange.Remove(enemy);
                // Optionally, stop shooting at the enemy here
            }
        }
    }

    private void OnDrawGizmos()
    {
        // Draw the tower's range in the editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, data.range);
    }
}
