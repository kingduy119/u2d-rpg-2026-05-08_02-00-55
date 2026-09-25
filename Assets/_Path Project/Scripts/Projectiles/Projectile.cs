using System;
using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    public class Projectile : MonoBehaviour,
        IPoolable<Projectile>
    {
        public IObjectPool<Projectile> Pool { get; set; }

        public ProjectileSO SO;
        protected ProjectileData Data;


        protected virtual void Update()
        {
            HandleMovement();
        }

        public virtual void Launch(Vector3 shootDirection)
        {

            Data = SO.Data.Clone();
            Data.Direction = shootDirection;
        }

        protected virtual void HandleMovement()
        {

            if (Data.LifeTime <= 0)
            {
                Deactivate();
                return;
            }
            Data.LifeTime -= Time.deltaTime;
            transform.position += Data.Speed * Time.deltaTime * Data.Direction;
        }

        public virtual void Deactivate()
        {
            if (gameObject.activeSelf) Pool.Release(this);
        }
    }

}