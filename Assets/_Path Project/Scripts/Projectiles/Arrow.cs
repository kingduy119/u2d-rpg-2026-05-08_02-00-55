
using UnityEngine;

namespace TDGame
{
    public class Arrow : Projectile
    {
        public override ProjectileType Type => ProjectileType.Arrow;


        public override void Launch(TowerSO data, Vector3 shotDirection)
        {
            base.Launch(data, shotDirection);
            RotateArrow();
        }

        private void RotateArrow()
        {
            float angle = Mathf.Atan2(_shotDirection.y, _shotDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}