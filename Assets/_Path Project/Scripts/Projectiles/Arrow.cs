using UnityEngine;
using UnityEngine.Pool;

namespace TDGame
{
    
    public class Arrow : Projectile
    {
        public override ProjectileType Type => ProjectileType.Arrow;
    }

}