using UnityEngine;

public class Projectile : MonoBehaviour
{
    private TowerData _data;
    private Vector3 _shotDirection;
    private float _projectileDuration;


    // Update is called once per frame
    void Update()
    {
        if (_projectileDuration <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            _projectileDuration -= Time.deltaTime;
            transform.position += new Vector3(_shotDirection.x, _shotDirection.y) * _data.projectileSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TDEnemy enemy = collision.GetComponent<TDEnemy>();
            enemy.TakeDamge(_data);
            gameObject.SetActive(false);
        }
    }

    public void Shoot(TowerData data, Vector3 shotDirection)
    {
        _data = data;
        _shotDirection = shotDirection;
        _projectileDuration = data.projectileDuration;
    }
}
