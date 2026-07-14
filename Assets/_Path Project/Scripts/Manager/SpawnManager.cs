using UnityEngine;
using System;

namespace TDGame
{
    public class SpawnManager : PersistentSingleton<SpawnManager>
    {
        public static event Action<int, int> OnWaveChanged;

        private EnemyFactory _enemyFactory;
        private SpawnState _spawnState = new();
        private WaveData[] Waves => LevelManager.Instance.LevelSO.waves;
        private WaveData Wave => Waves[_spawnState.WaveCount];

        public Path MapPath;
        private Transform _spawnPoint;

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
            GameEvent.OnEnemyReachedEnd += _spawnState.HandlePointReachedEnd;
            GameEvent.OnEnemyDie += _spawnState.HandleEnemyDie;
        }
        private void OnDisable()
        {
            GameEvent.OnEnemyReachedEnd -= _spawnState.HandlePointReachedEnd;
            GameEvent.OnEnemyDie -= _spawnState.HandleEnemyDie;
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

        public void StartWave()
        {
            MapPath = GameObject.Find("MapPath").GetComponent<Path>();
            _spawnPoint = MapPath.wayPoints[0].transform;
            _spawnState.Start();
        }
        private void SpawnObject()
        {
            Enemy obj = _enemyFactory.GetObject(Wave.enemyType);
            if (obj == null) return;

            obj.transform.position = _spawnPoint.position;
            obj.gameObject.SetActive(true);
        }
    }

}
