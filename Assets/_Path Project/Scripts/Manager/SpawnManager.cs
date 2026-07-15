using UnityEngine;
using System;

namespace TDGame
{
    public class SpawnManager : PersistentSingleton<SpawnManager>
    {

        private EnemyFactory _enemyFactory;
        private InGameState InGameState => GameManager.Instance.InGameState;
        private WaveData[] Waves => LevelManager.Instance.LevelSO.waves;
        private WaveData CurrentWave => Waves[InGameState.WaveCount];
        private SpawnState _spawnState = new();

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
            InGameState.OnStartWave += HandleStartWave;
        }
        private void OnDisable()
        {
            InGameState.OnStartWave -= HandleStartWave;
        }

        void Update()
        {
            if (!InGameState.IsStarted) return;

            if (_spawnState.ForEachTimer(CurrentWave.perway))
            {
                _spawnState.RefreshTimer();
                SpawnObject();
                return;
            }

            if (InGameState.Enemies <= 0)
            {
                _spawnState.ResetCount();
                InGameState.NextWave();

                if (InGameState.WaveCount >= Waves.Length)
                {
                    GameEvent.SendMissionComplete();
                }
            }
        }

        public void HandleStartWave()
        {
            MapPath = GameObject.Find("MapPath").GetComponent<Path>();
            _spawnPoint = MapPath.wayPoints[0].transform;

            InGameState.Enemies = CurrentWave.perway;
            _spawnState.UpdateSpawn(CurrentWave);
        }
        private void SpawnObject()
        {
            Enemy obj = _enemyFactory.GetObject(CurrentWave.enemyType);
            if (obj == null) return;

            obj.transform.position = _spawnPoint.position;
            obj.gameObject.SetActive(true);
        }
    }

}
