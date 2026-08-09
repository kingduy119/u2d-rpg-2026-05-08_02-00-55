

using System;
using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "ProjectileSO", menuName = "Game TD/ProjectileSO")]
    public class ProjectileSO : ScriptableObject
    {
        public Sprite Sprite;
        // public ProjectileType Type;
        public ProjectileData Data;
    }

    [Serializable]
    public class ProjectileData : DirtyState
    {
        [SerializeField] private float m_Damage;
        [SerializeField] private float m_Speed;
        [SerializeField] public float m_LifeTime;

        public float Damage
        {
            get => m_Damage;
            set => SetValue(ref m_Damage, value);
        }
        public float Speed
        {
            get => m_Speed;
            set => SetValue(ref m_Speed, value);
        }
        public float LifeTime
        {
            get => m_LifeTime;
            set => SetValue(ref m_Speed, value);
        }

        public Vector3 Direction { get; set; }

        public ProjectileData() { }
        public ProjectileData(ProjectileData item)
        {
            m_Damage = item.Damage;
            m_Speed = item.Speed;
            m_LifeTime = item.LifeTime;
            Direction = item.Direction;
        }

        public ProjectileData Clone()
        {
            return new ProjectileData(this);
        }
    }
}