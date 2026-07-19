
using UnityEngine;

namespace TDGame
{
    public class Arrow : Projectile
    {
        public override ProjectileType Type => ProjectileType.Arrow;

        public override void Launch(Vector3 shotDirection)
        {
            base.Launch(shotDirection);
            RotateArrow();
        }

        private void RotateArrow()
        {
            float angle = Mathf.Atan2(Data.Direction.y, Data.Direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }
}