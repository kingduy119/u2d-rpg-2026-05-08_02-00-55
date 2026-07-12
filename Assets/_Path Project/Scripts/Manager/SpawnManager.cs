using UnityEngine;
using System;

namespace TDGame
{

    public class SpawnState
    {
        private int _count = 0;
        private int _waveCount = 0;
        private float _timer = 0f;
        private float _interval = 1f;
        private int _removedEnemies = 0;
        public bool IsStarted { get; private set; } = false;
        public bool IsDirty { get; private set; } = false;
        public int WaveCount
        {
            get => _waveCount;
            set
            {
                _waveCount = value;
                IsDirty = true;
            }
        }
        public int EnemyCount
        {
            get => _removedEnemies;
            set
            {
                _removedEnemies = value;
                IsDirty = true;
            }
        }

        public bool CheckSpawnTimer(int perway)
        {
            _timer -= Time.deltaTime;
            return _timer <= 0f && _count < perway;
        }

        public void Start()
        {
            IsStarted = true;
        }

        public void RefreshTimer()
        {
            _timer = _interval;
            _count++;
        }

        public void Stop()
        {
            IsStarted = false;
            _removedEnemies = 0;
            _count = 0;
        }
    }

    public class SpawnManager : PersistentSingleton<SpawnManager>
    {
        public static event Action<int, int> OnWaveChanged;

        private EnemyFactory _enemyFactory;
        private SpawnState _spawnState = new();
        private WaveData[] Waves => LevelManager.Instance.LevelSO.waves;
        private WaveData Wave => Waves[_spawnState.WaveCount];

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            _enemyFactory = FactoryManager.Instance.EnemyFactory;
        }

        private void OnEnable()
        {
            GameEvent.OnEnemyReachedEnd += HandlePointReachedEnd;
            GameEvent.OnEnemyDie += HandleEnemyDie;
        }
        private void OnDisable()
        {
            GameEvent.OnEnemyReachedEnd -= HandlePointReachedEnd;
            GameEvent.OnEnemyDie -= HandleEnemyDie;
        }

        void Update()
        {
            if (_spawnState.IsDirty)
            {
                OnWaveChanged?.Invoke(Wave.perway - _spawnState.EnemyCount, _spawnState.WaveCount + 1);
            }

            if (!_spawnState.IsStarted) return;

            if (_spawnState.CheckSpawnTimer(Wave.perway))
            {
                _spawnState.RefreshTimer();
                SpawnObject();
                return;
            }

            if (_spawnState.EnemyCount >= Wave.perway)
            {
                _spawnState.Stop();
                _spawnState.WaveCount++;

                if (_spawnState.WaveCount >= Waves.Length)
                {
                    GameEvent.SendMissionComplete();
                }
            }
        }

        public void StartWave() => _spawnState.Start();
        private void SpawnObject()
        {
            Enemy obj = _enemyFactory.GetObject(Wave.enemyType);
            if (obj == null) return;

            obj.transform.position = transform.position;
            obj.gameObject.SetActive(true);
        }


        private void HandlePointReachedEnd(EnemyData pointData)
        {
            _spawnState.EnemyCount++;
        }

        private void HandleEnemyDie(Enemy enemy)
        {
            // RemovedEnemies++;
            _spawnState.EnemyCount++;
        }
    }

}


// [Header("Spawn Config")]
// public float _spawnTimer = 0f;
// public float _spawnInterval = 1f;


// public bool IsDirty { get; private set; } = true;
// private bool IsWaveActive = false;
// private int _spawnedCount = 0;
// private int _removedEnemies = 0;
// public int RemovedEnemies
// {
//     get => _removedEnemies;
//     set
//     {
//         _removedEnemies = value;
//         IsDirty = true;
//     }
// }

// private int _waveIndex = 0;
// public int WaveCount
// {
//     get => _waveIndex;
//     set
//     {
//         _waveIndex = value;
//         IsDirty = true;
//     }
// }




// if (IsDirty)
// {
//     OnWaveChanged?.Invoke(Wave.perway - _removedEnemies, _waveIndex + 1);
//     IsDirty = false;
// }

// if (!IsWaveActive) return;

// _spawnTimer -= Time.deltaTime;
// if (_spawnTimer <= 0f && _spawnedCount < Wave.perway)
// {
//     _spawnTimer = _spawnInterval;
//     _spawnedCount++;
//     SpawnObject();
// }
// else if (RemovedEnemies >= Wave.perway)
// {
//     IsWaveActive = false;
//     _spawnedCount = 0;
//     WaveCount++;

//     if (WaveCount >= Waves.Length)
//     {
//         GameEvent.SendMissionComplete();
//     }
// }