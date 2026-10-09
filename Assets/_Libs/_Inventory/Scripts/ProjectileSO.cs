


using System;
using UnityEngine;

namespace Weapons
{
    [CreateAssetMenu(fileName = "ProjectileSO", menuName = "Items/ProjectileSO")]
    public class ProjectSO : ScriptableObject
    {
        public ProjectileData Data;
    }

    [Serializable]
    public class ProjectileData
    {
        public float Damage;
        public float Speed;
        public float LifeTime;

        public ProjectileData(ProjectileData data)
        {
            Damage = data.Damage;
            Speed = data.Speed;
            LifeTime = data.LifeTime;
        }
    }
}