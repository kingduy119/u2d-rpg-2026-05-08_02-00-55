using UnityEngine;
using System;

namespace TDGame
{
    public class Spawn
    {
        float m_timer = 0;
        float m_interval = 1;

        public int Count { get; private set; } = 0;
        public bool IsActive { get; private set; } = false;

        public void Update()
        {
            if (IsActive) return;

            m_timer += Time.deltaTime;
            if (m_timer >= m_interval) IsActive = true;
        }
        public void Reset()
        {
            m_timer = 0;
            IsActive = false;
        }
    }


    [RequireComponent(typeof(EnemyFactory))]
    public class SpawnManager : PersistentSingleton<SpawnManager>
    {
        public static event Action<int, int> OnWaveChanged;
        public static event Action OnMissionComplete;


        [Header("Spawn Config")]
        public float _spawnTimer = 0f;
        public float _spawnInterval = 1f;


        public bool IsDirty { get; private set; } = true;
        private bool IsWaveActive = false;
        private int _spawnedCount = 0;
        private int _removedEnemies = 0;
        public int RemovedEnemies
        {
            get => _removedEnemies;
            set
            {
                _removedEnemies = value;
                IsDirty = true;
            }
        }

        private int _waveIndex = 0;
        public int WaveCount
        {
            get => _waveIndex;
            set
            {
                _waveIndex = value;
                IsDirty = true;
            }
        }


        public Spawn m_spawner;
        private EnemyFactory m_enemyFactor;
        private WaveData[] Waves => LevelManager.Instance.LevelSO.waves;
        private WaveData Wave => LevelManager.Instance.LevelSO.waves[_waveIndex];


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
            if (IsDirty)
            {
                OnWaveChanged?.Invoke(Wave.perway - _removedEnemies, _waveIndex + 1);
                IsDirty = false;
            }

            if (!IsWaveActive) return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f && _spawnedCount < Wave.perway)
            {
                _spawnTimer = _spawnInterval;
                _spawnedCount++;
                SpawnObject();
            }
            else if (RemovedEnemies >= Wave.perway)
            {
                IsWaveActive = false;
                _spawnedCount = 0;
                WaveCount++;

                if (WaveCount >= Waves.Length)
                {
                    OnMissionComplete?.Invoke();
                }
            }
        }

        public void StartWave()
        {
            IsWaveActive = true;
            RemovedEnemies = 0;
        }

        private void SpawnObject()
        {
            Enemy obj = m_enemyFactor.GetEnemy(Wave.enemyType);
            if (obj == null) return;

            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);
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