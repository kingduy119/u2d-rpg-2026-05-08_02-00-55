using UnityEngine;
using System;
using System.Collections.Generic;

namespace TDGame
{
    [RequireComponent(typeof(EnemyFactory))]
    public class SpawnManager : PersistentSingleton<SpawnManager>
    {
        public static event Action<int> OnWaveChanged;
        public static event Action OnMissionComplete;

        public float _spawnTimer = 0f;
        public float _spawnInterval = 1f;
        // private float _waveCooldown = 3f;
        private bool _isWaveActive = false;

        private int _spawnedCount = 0;
        private int _enemiesRemoved = 0;
        private int _waveIndex = 0;
        // public WaveData[] waves;
        // private WaveData[] _waves => LevelManager.Instance.CurrentLevel.waves;
        private WaveData[] _waves => LevelManager.Instance.Level.waves;
        private WaveData CurrentWave => _waves[_waveIndex];

        EnemyFactory m_enemyFactor;

        public bool ActiveWave => _isWaveActive;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            m_enemyFactor = GetComponent<EnemyFactory>();
        }

        private void OnEnable()
        {
            Enemy.OnEnemyReachedEnd += HandlePointReachedEnd;
            GameEvent.OnEnemyDie += HandleEnemyDie;
        }
        private void OnDisable()
        {
            Enemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
            GameEvent.OnEnemyDie -= HandleEnemyDie;
        }

        private void Start()
        {
            OnWaveChanged?.Invoke(_waveIndex);
        }

        void Update()
        {
            if (!_isWaveActive) return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f && _spawnedCount < CurrentWave.perway)
            {
                _spawnTimer = _spawnInterval;
                SpawnObject();
            }
            else if (_enemiesRemoved >= CurrentWave.perway)
            {
                _waveIndex += 1;
                _spawnedCount = 0;
                _enemiesRemoved = 0;
                _isWaveActive = false;
                OnWaveChanged?.Invoke(_waveIndex);

                if (_waveIndex >= _waves.Length)
                    OnMissionComplete?.Invoke();
            }
        }

        public void StartNewWave()
        {
            if (_waveIndex < _waves.Length)
                _isWaveActive = true;
        }

        private void SpawnObject()
        {
            // GameObject obj = poolDictionary[CurrentWave.enemyType].GetObject();
            Enemy obj = m_enemyFactor.GetEnemy(CurrentWave.enemyType);
            if (obj == null) return;

            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);
            _spawnedCount++;
        }


        private void HandlePointReachedEnd(EnemyData pointData)
        {
            _enemiesRemoved++;
        }

        private void HandleEnemyDie(Enemy enemy)
        {
            _enemiesRemoved++;
        }
    }

}