using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class Projectile : MonoBehaviour
    {
        private TowerData _data;
        private Vector3 _shotDirection;
        private float _projectileDuration;

        private IObjectPool<Projectile> _objectPool;
        public IObjectPool<Projectile> ObjectPool { set => _objectPool = value; }


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
                transform.position += _data.projectileSpeed * Time.deltaTime * new Vector3(_shotDirection.x, _shotDirection.y);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy_Health enemy = collision.GetComponent<TDGame.Enemy_Health>();
                enemy.TakeDamge(_data);

                Deactivate();
            }
        }

        public void Shoot(TowerData data, Vector3 shotDirection)
        {
            _data = data;
            _shotDirection = shotDirection;
            _projectileDuration = data.projectileDuration;
        }

        public void Deactivate()
        {
            _objectPool.Release(this);
        }
    }
}