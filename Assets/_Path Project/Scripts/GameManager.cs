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
        public int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                OnLivesChanged?.Invoke(_lives);
            }
        }

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

        private int _rocks = 0;
        public int Rocks
        {
            get => _rocks;
            set
            {
                _rocks = value;
                OnGoldsChanged?.Invoke(_rocks);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlayMainMenuMusic();
        }

        void OnEnable()
        {
            Enemy.OnEnemyReachedEnd += HandlePointReachedEnd;
            Enemy.OnGetEnemyReward += HandleGetEnemyReward;
            SceneManager.sceneLoaded += OnSceneLoaded;
            LevelManager.OnLoadLevel += HandleLoadLevel;
        }

        void OnDisable()
        {
            Enemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
            Enemy.OnGetEnemyReward -= HandleGetEnemyReward;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            LevelManager.OnLoadLevel -= HandleLoadLevel;
        }

        void Start()
        {
            OnLivesChanged?.Invoke(_lives);
            OnGoldsChanged?.Invoke(_golds);

            LoadScece();
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
        public void SetTimeScale(float scale) => Time.timeScale = scale;

        public void SpendGold(int amount)
        {
            if (Golds >= amount)
                Golds -= amount;
            else
            {
                Debug.LogWarning("Not enough gold!");
            }
        }

        private void LoadScece() => GameEvent.LoadScene(SceneManager.GetActiveScene().name);
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => GameEvent.LoadScene(SceneManager.GetActiveScene().name);

        private void HandleLoadLevel(LevelSO data)
        {
            Golds = data.startingGold;
            Lives = data.startingLives;

        }
    }

}