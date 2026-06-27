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
        private WaveData[] Waves => LevelManager.Instance.Waves;
        private WaveData Wave => LevelManager.Instance.Waves[_waveIndex];


        private bool _isWaveActive = false;
        private int _spawnedCount = 0;
        private int _removedEnemies = 0;
        public int RemovedEnemies
        {
            get => _removedEnemies;
            set
            {
                _removedEnemies = value;
                UIController.Instance.UpdateEnemies(Wave.perway - _removedEnemies);
            }
        }

        private int _waveIndex = 0;
        public int WaveIndex
        {
            get => _waveIndex;
            set
            {
                _waveIndex = value;
                OnWaveChanged?.Invoke(_waveIndex, Waves.Length);
            }
        }

        public bool ActiveWave => _isWaveActive;
        public int AliveEnemies => Wave.perway - _removedEnemies;



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
            OnWaveChanged?.Invoke(_waveIndex, Waves.Length);
            RemovedEnemies = 0;
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
                RemovedEnemies = 0;
                _spawnedCount = 0;
                WaveIndex++;

                if (_waveIndex >= Waves.Length)
                {
                    OnMissionComplete?.Invoke();
                }
            }
        }

        public void StartNewWave()
        {
            if (_waveIndex < Waves.Length)
                _isWaveActive = true;
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