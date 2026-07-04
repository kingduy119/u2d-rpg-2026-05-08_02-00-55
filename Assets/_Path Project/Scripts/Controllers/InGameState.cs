using System;
using UnityEngine;

namespace TDGame
{
    public class InGameState
    {
        public static event Action OnUpdateInGameUI;

        public bool IsDirty { get; private set; } = false;

        private float m_maxGameSpeed = 3f;
        private float m_gameSpeed = 1f;
        public float GameSpeed
        {
            get => m_gameSpeed;
            set
            {
                m_gameSpeed = value >= m_maxGameSpeed ? 1 : value;
                IsDirty = true;
            }
        }

        private int m_lives = 0;
        public int Lives
        {
            get => m_lives;
            set
            {
                m_lives = value;
                IsDirty = true;
            }
        }

        private int m_golds = 0;
        public int Golds
        {
            get => m_golds;
            set
            {
                m_golds = value;
                IsDirty = true;
            }
        }

        private int m_rocks = 0;
        public int Rocks
        {
            get => m_rocks;
            set
            {
                m_rocks = value;
                IsDirty = true;
            }
        }

        private int m_wood = 0;
        public int Woods
        {
            get => m_wood;
            set
            {
                m_wood = value;
                IsDirty = true;
            }
        }

        private int m_wave = 1;
        public int WaveCount
        {
            get => m_wave;
            set
            {
                m_wave = value;
                IsDirty = true;
            }
        }

        private int m_enemies = 0;
        public int Enemies
        {
            get => m_enemies;
            set
            {
                m_enemies = value;
                IsDirty = true;
            }
        }
        public InGameState() { }
        public void HandlePointReachedEnd(EnemyData enemy)
        {
            Lives -= enemy.damage;
            if (Lives <= 0)
            {
                Debug.Log("Game Over!");
            }
        }

        public void HandleWaveChanged(int enemies, int wave)
        {
            Enemies = enemies;
            WaveCount = wave;
        }


        public void HandleGetEnemyReward(EnemyData enemy) => Golds += enemy.goldReward;
        public void MarkDirty() => IsDirty = true;
        public void Clearn() => IsDirty = false;
    }
}