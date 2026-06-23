using UnityEngine;
using System;
using System.Collections.Generic;

namespace TDGame
{
    public class SpawnManager : PersistentSingleton<SpawnManager>
    {
        // public static SpawnManager Instance { get; set; }
        public static event Action<int> OnWaveChanged;
        public static event Action OnMissionComplete;

        public float _spawnTimer = 0f;
        public float _spawnInterval = 1f;
        // private float _waveCooldown = 3f;
        private bool _isWaveActive = false;

        private int _spawnedCount = 0;
        private int _enemiesRemoved = 0;
        private int _currentWaveIndex = 0;
        // public WaveData[] waves;
        private WaveData[] _waves => LevelManager.Instance.CurrentLevel.waves;
        private WaveData CurrentWave => _waves[_currentWaveIndex];

        [SerializeField] private Object_Pool basicPool;
        [SerializeField] private Object_Pool normalPool;
        [SerializeField] private Object_Pool fastPool;
        [SerializeField] private Object_Pool mumyOrcPool;
        private Dictionary<EnemyType, Object_Pool> poolDictionary;

        public bool ActiveWave => _isWaveActive;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            poolDictionary = new Dictionary<EnemyType, Object_Pool>()
        {
            { EnemyType.MummyOrc, mumyOrcPool },
            { EnemyType.Basic, basicPool },
            { EnemyType.Normal, normalPool },
            { EnemyType.Fast, fastPool }
        };
        }

        private void OnEnable()
        {
            Enemy.OnEnemyReachedEnd += HandlePointReachedEnd;
            Enemy.OnEnemyDestroyed += HandleEnemyDestroyed;
        }
        private void OnDisable()
        {
            Enemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
            Enemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
        }

        private void Start()
        {
            OnWaveChanged?.Invoke(_currentWaveIndex);
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
                _currentWaveIndex += 1;
                _spawnedCount = 0;
                _enemiesRemoved = 0;
                _isWaveActive = false;
                OnWaveChanged?.Invoke(_currentWaveIndex);

                if (_currentWaveIndex >= _waves.Length)
                    OnMissionComplete?.Invoke();
                // 
            }
        }

        public void StartNewWave()
        {
            if (_currentWaveIndex < _waves.Length)
                _isWaveActive = true;
        }

        private void SpawnObject()
        {
            GameObject obj = poolDictionary[CurrentWave.enemyType].GetObject();
            obj.transform.position = transform.position;
            obj.SetActive(true);
            _spawnedCount++;
        }


        private void HandlePointReachedEnd(EnemyData pointData)
        {
            _enemiesRemoved++;
        }

        private void HandleEnemyDestroyed(Enemy enemy)
        {
            _enemiesRemoved++;
        }
    }

}