using UnityEngine;
using System;

namespace TDGame
{
    public class SpawnManager : PersistentSingleton<SpawnManager>
    {
        private EnemyFactory EnemyFactory => FactoryManager.Instance.EnemyFactory;
        private InGameState InGameState;
        private LevelState LevelState;
        private WaveData[] Waves => LevelState.CurrentLevel.waves;
        private WaveData CurrentWave => Waves[InGameState.WaveCount];
        private readonly SpawnState _spawnState = new();

        [NonSerialized]
        public Path MapPath;
        private Transform _spawnPoint;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            InGameState.OnStartWave += HandleStartWave;
        }
        private void OnDisable()
        {
            InGameState.OnStartWave -= HandleStartWave;
        }

        void Start()
        {
            InGameState = GameManager.Instance.InGameState;
            LevelState = LevelManager.Instance.LevelState;
        }

        void Update()
        {
            if (InGameState == null) return;

            if (!InGameState.IsStarted) return;

            if (_spawnState.RunTimer(CurrentWave.perway))
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
            Enemy obj = EnemyFactory.GetObject(CurrentWave.enemyType);
            if (obj == null) return;

            obj.transform.position = _spawnPoint.position;
            obj.gameObject.SetActive(true);
        }
    }

}
