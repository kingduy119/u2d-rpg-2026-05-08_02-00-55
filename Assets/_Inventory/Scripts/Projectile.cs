
using UnityEngine;
using UnityEngine.Pool;

namespace Weapons
{
    public class Projectile : MonoBehaviour
    , IPoolable<Projectile>
    {
        public IObjectPool<Projectile> Pool { get; set; }
        public ProjectSO SO;
        ProjectileData Data;
        Vector3 Direction;

        float _lifeTime;
        void Awake()
        {
            Data = new(SO.Data);
        }

        public void Launch(Transform parent, Vector2 direction)
        {
            transform.position = parent.position;
            Direction = direction;
            RotateArrow();
            _lifeTime = Data.LifeTime;
        }

        protected virtual void Update()
        {
            if (_lifeTime <= 0)
            {
                Deactivate();
                return;
            }
            _lifeTime -= Time.deltaTime;
            transform.position += Data.Speed * Time.deltaTime * Direction;
        }

        private void RotateArrow()
        {
            float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public virtual void Deactivate()
        {
            if (gameObject.activeSelf) Pool.Release(this);
        }
    }

}
