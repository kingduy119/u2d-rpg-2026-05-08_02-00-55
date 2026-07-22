using System;
using UnityEngine;

namespace TDGame
{
    public class GameState : DirtyState
    {
        public static Action OnStartWave;

        // public bool IsDirty { get; private set; } = false;

        private bool _isStarted = false;
        public bool IsStarted
        {
            get => _isStarted;
            set
            {
                _isStarted = value;
                IsDirty = true;
            }
        }

        private float _maxGameSpeed = 3f;
        public float MaxGameSpeed
        {
            get => _maxGameSpeed;
            set => SetValue(ref _maxGameSpeed, value);
        }

        private float _gameSpeed = 1f;
        public float GameSpeed
        {
            get => _gameSpeed;
            set
            {
                SetValue(ref _gameSpeed, value);
                _gameSpeed = Mathf.Clamp(_gameSpeed % _maxGameSpeed, 1, _maxGameSpeed);
                Time.timeScale = _gameSpeed;
            }
        }

        private int _lives = 0;
        public int Lives
        {
            get => _lives;
            set
            {
                _lives = value;
                IsDirty = true;
            }
        }

        private int _golds = 0;
        public int Golds
        {
            get => _golds;
            set
            {
                _golds = value;
                IsDirty = true;
            }
        }

        private int _rocks = 0;
        public int Rocks
        {
            get => _rocks;
            set
            {
                _rocks = value;
                IsDirty = true;
            }
        }

        private int _wood = 0;
        public int Woods
        {
            get => _wood;
            set
            {
                _wood = value;
                IsDirty = true;
            }
        }

        private int _wave = 0;
        public int WaveCount
        {
            get => _wave;
            set
            {
                _wave = value;
                IsDirty = true;
            }
        }

        private int _enemies = 0;
        public int Enemies
        {
            get => _enemies;
            set
            {
                _enemies = value;
                IsDirty = true;
            }
        }

        public GameState() { }

        public void OnEnable()
        {
            GameEvent.OnEnemyReachedEnd += HandlePointReachedEnd;
            GameEvent.OnEnemyDie += HandleEnemyDie;
            GameEvent.OnGetEnemyReward += HandleGetEnemyReward;
        }

        public void OnDisable()
        {
            GameEvent.OnEnemyReachedEnd -= HandlePointReachedEnd;
            GameEvent.OnEnemyDie -= HandleEnemyDie;
            GameEvent.OnGetEnemyReward -= HandleGetEnemyReward;
        }

        public void StartWave()
        {
            IsStarted = true;
            OnStartWave?.Invoke();
        }

        public void NextWave()
        {
            IsStarted = false;
            WaveCount++;
        }
        public void HandlePointReachedEnd(Enemy enemy)
        {
            Enemies--;
            Lives -= enemy.Data.damage;
            if (Lives <= 0)
            {
                Debug.Log("Show GameOver!");
            }
        }

        public void ResetOnLoadScene()
        {
            WaveCount = 0;
        }

        public void ChangeGameSpeed() => GameSpeed++;
        public void HandleEnemyDie(Enemy _) => Enemies--;
        public void HandleGetEnemyReward(Enemy enemy) => Golds += enemy.Data.goldReward;

        public bool CheckAndSpendResource(TowerSO towerData)
        {
            if (Golds >= towerData.cost)
            {
                Golds -= towerData.cost;
                return true;
            }
            return false;
        }
    }
}