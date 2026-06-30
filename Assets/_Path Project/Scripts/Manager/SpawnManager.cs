using UnityEngine;
using System;

namespace TDGame
{
    [RequireComponent(typeof(EnemyFactory))]
    public class SpawnManager : PersistentSingleton<SpawnManager>
    {
        public static event Action<int, int> OnWaveChanged;
        public static event Action OnMissionComplete;

        [Header("Spawn Config")]
        public float _spawnTimer = 0f;
        public float _spawnInterval = 1f;

        // Private
        private EnemyFactory m_enemyFactor;
        private WaveData[] Waves => LevelManager.Instance.LevelSO.waves;
        private WaveData Wave => LevelManager.Instance.LevelSO.waves[_waveIndex];

        private bool _isWaveActive = false;
        private int _spawnedCount = 0;
        private int _removedEnemies = 0;
        public int RemovedEnemies
        {
            get => _removedEnemies;
            set
            {
                _removedEnemies = value;
                int alives = Mathf.Clamp(Wave.perway - _removedEnemies, 0, Wave.perway);
                UIController.Instance.UpdateEnemies(alives);
            }
        }

        private int _waveIndex = 0;
        public int WaveIndex
        {
            get => _waveIndex;
            set
            {
                _waveIndex = value;
                OnWaveChanged?.Invoke(Mathf.Clamp(_waveIndex, 0, Waves.Length), Waves.Length);
            }
        }

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

        void Update()
        {
            if (!_isWaveActive) return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f && _spawnedCount < Wave.perway)
            {
                _spawnTimer = _spawnInterval;
                SpawnObject();
            }
            else if (RemovedEnemies >= Wave.perway)
            {
                _isWaveActive = false;
                // RemovedEnemies = 0;
                _spawnedCount = 0;
                WaveIndex++;

                if (WaveIndex >= Waves.Length)
                {
                    OnMissionComplete?.Invoke();
                }
            }
        }

        public void StartWave()
        {
            if (_waveIndex < Waves.Length)
                _isWaveActive = true;
            RemovedEnemies = 0;
        }

        private void SpawnObject()
        {
            Enemy obj = m_enemyFactor.GetEnemy(Wave.enemyType);
            if (obj == null) return;

            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);
            _spawnedCount++;
        }


        private void HandlePointReachedEnd(EnemyData pointData)
        {
            RemovedEnemies++;
        }

        private void HandleEnemyDie(Enemy enemy)
        {
            RemovedEnemies++;
        }
    }

}