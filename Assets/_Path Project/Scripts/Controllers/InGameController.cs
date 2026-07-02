using System;
using UnityEngine;

namespace TDGame
{
    public class InGameController
    {
        public static event Action OnUpdateInGameUI;

        private bool m_isDirty = false;
        private float m_maxGameSpeed = 3f;
        private float m_gameSpeed = 1f;
        public float GameSpeed
        {
            get => m_gameSpeed;
            set
            {
                m_gameSpeed = value >= m_maxGameSpeed ? 1 : value;
                m_isDirty = true;
            }
        }

        private int m_lives = 0;
        public int Lives
        {
            get => m_lives;
            set
            {
                m_lives = value;
                m_isDirty = true;
            }
        }

        private int m_golds = 0;
        public int Golds
        {
            get => m_golds;
            set
            {
                m_golds = value;
                m_isDirty = true;
            }
        }

        private int m_rocks = 0;
        public int Rocks
        {
            get => m_rocks;
            set
            {
                m_rocks = value;
                m_isDirty = true;
            }
        }

        private int m_wood = 0;
        public int Woods
        {
            get => m_wood;
            set
            {
                m_wood = value;
                m_isDirty = true;
            }
        }

        public InGameController() { }
        public void Update()
        {
            if (m_isDirty)
            {
                OnUpdateInGameUI?.Invoke();
                m_isDirty = false;
            }
        }

        public void RegisterEvent()
        {
            // GameEvent.OnPauseInGame += HandlePauseInGame;
            // GameEvent.OnResumeInGame += HandleResumeInGame;
            Enemy.OnEnemyReachedEnd += HandlePointReachedEnd;
            Enemy.OnGetEnemyReward += HandleGetEnemyReward;

        }

        public void UnregisterEvent()
        {
            // GameEvent.OnPauseInGame -= HandlePauseInGame;
            // GameEvent.OnResumeInGame -= HandleResumeInGame;
            Enemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
            Enemy.OnGetEnemyReward -= HandleGetEnemyReward;
        }

        private void HandlePointReachedEnd(EnemyData enemy)
        {
            Lives -= enemy.damage;
            if (Lives <= 0)
            {
                Debug.Log("Game Over!");
            }
        }

        private void HandleGetEnemyReward(EnemyData enemy) => Golds += enemy.goldReward;

        // public void HandlePauseInGame()
        // {
        //     m_isPaused = true;
        //     Time.timeScale = 0f;
        //     AudioManager.Instance.PlayPauseSound();
        // }

        // public void HandleResumeInGame()
        // {
        //     m_isPaused = false;
        //     Time.timeScale = m_gameSpeed;
        //     AudioManager.Instance.PlayResumeSound();
        // }
    }
}