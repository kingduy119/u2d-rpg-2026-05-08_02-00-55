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
        // private InGameState InGameState => GameManager.Instance.InGameState;
        private InGameState InGameState => GameManager.Instance.InGameState;

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
            // GameEvent.OnEnemyReachedEnd += _spawnState.HandlePointReachedEnd;
            // GameEvent.OnEnemyDie += _spawnState.HandleEnemyDie;
            InGameState.OnStartWave += HandleStartWave;
        }
        private void OnDisable()
        {
            // GameEvent.OnEnemyReachedEnd -= _spawnState.HandlePointReachedEnd;
            // GameEvent.OnEnemyDie -= _spawnState.HandleEnemyDie;
            InGameState.OnStartWave -= HandleStartWave;
        }

        void Update()
        {
            if (_spawnState.IsDirty)
            {
                OnWaveChanged?.Invoke(Wave.perway - _spawnState.EnemyCount, _spawnState.WaveCount + 1);
            }

            // if (!_spawnState.IsStarted) return;
            if (!InGameState.IsStarted) return;

            if (_spawnState.ForEachTimer(Wave.perway))
            {
                _spawnState.RefreshTimer();
                SpawnObject();
                return;
            }

            if (_spawnState.EnemyCount >= Wave.perway)
            {
                _spawnState.ResetNewWave();

                if (_spawnState.WaveCount >= Waves.Length)
                {
                    GameEvent.SendMissionComplete();
                }
            }
        }

        public void HandleStartWave()
        {
            MapPath = GameObject.Find("MapPath").GetComponent<Path>();
            _spawnPoint = MapPath.wayPoints[0].transform;
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
