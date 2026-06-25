using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class TDGameManager : PersistentSingleton<TDGameManager>
    {
        public static event Action<int> OnLivesChanged;
        public static event Action<int> OnGoldsChanged;

        private int _lives = 20;
        public int Lives => _lives;

        private int _golds = 0;

        public int Golds
        {
            get => _golds;
            set
            {
                _golds = value;
                OnGoldsChanged?.Invoke(_golds);
            }
        }

        void OnEnable()
        {
            Enemy.OnEnemyReachedEnd += HandlePointReachedEnd;
            Enemy.OnGetEnemyReward += HandleGetEnemyReward;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            Enemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
            Enemy.OnGetEnemyReward -= HandleGetEnemyReward;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Start()
        {
            OnLivesChanged?.Invoke(_lives);
            OnGoldsChanged?.Invoke(_golds);
        }

        private void HandlePointReachedEnd(EnemyData enemy)
        {
            _lives -= enemy.damage;
            OnLivesChanged?.Invoke(_lives);

            if (_lives <= 0)
            {
                Debug.Log("Game Over!");
            }
        }

        private void HandleGetEnemyReward(EnemyData enemy)
        {
            Golds += enemy.goldReward;
        }


        public void SetTimeScale(float scale)
        {
            Time.timeScale = scale;
        }

        public void SpendGold(int amount)
        {
            if (_golds >= amount)
            {
                _golds -= amount;
                OnGoldsChanged?.Invoke(_golds);
            }
            else
            {
                Debug.LogWarning("Not enough gold!");
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "TD_MainMenu")
            {
                AudioManager.Instance.PlayMainMenuMusic();
            }
            else if (LevelManager.Instance != null && LevelManager.Instance.Level != null)
            {
                AudioManager.Instance.PlayGameplayMusic();
            }
        }
    }

}