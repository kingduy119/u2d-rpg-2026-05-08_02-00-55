using UnityEngine;
using System;

namespace TDGame
{
    public class SpawnManager : MonoBehaviour
    {
        private EnemyFactory EnemyFactory => GameManager.Instance.FactoryManager.EnemyFactory;
        private GameState GameState => GameManager.Instance.GameState;
        private LevelState LevelState => GameManager.Instance.LevelManager.LevelState;
        private WaveData[] Waves => LevelState.CurrentLevel.waves;
        private WaveData CurrentWave => Waves[GameState.WaveCount];
        private readonly SpawnState _spawnState = new();

        [NonSerialized]
        public Path MapPath;
        private Transform _spawnPoint;

        private void OnEnable()
        {
            GameState.OnStartWave += HandleStartWave;
        }
        private void OnDisable()
        {
            GameState.OnStartWave -= HandleStartWave;
        }

        void Update()
        {
            if (GameState == null || !GameState.IsStarted) return;

            if (_spawnState.RunTimer(CurrentWave.perway))
            {
                _spawnState.RefreshTimer();
                SpawnObject();
                return;
            }

            if (GameState.Enemies <= 0)
            {
                _spawnState.ResetCount();
                GameState.NextWave();

                if (GameState.WaveCount >= Waves.Length)
                {
                    GameEvent.SendMissionComplete();
                }
            }
        }

        public void HandleStartWave()
        {
            MapPath = GameObject.Find("MapPath").GetComponent<Path>();
            _spawnPoint = MapPath.wayPoints[0].transform;

            GameState.Enemies = CurrentWave.perway;
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
