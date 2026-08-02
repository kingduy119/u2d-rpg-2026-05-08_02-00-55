

using System;
using Unity.VisualScripting;
using UnityEngine;

namespace TDGame
{
    [Serializable]
    public class SpawnConfig
    {
        public EnemySO EnemyType;
        public float Duration;
        public float Interval;
        [NonSerialized]
        public float Timer;
    }


    public class Spawner : MonoBehaviour
    {
        public GameObject[] Pathway;
        private SpawnConfig _SpawnConfig;
        private FactoryManager FactoryManager;

        private bool IsInitialized = false;
        private float _spawnTimer = 0f;

        public void Start()
        {
            FactoryManager = GameManager.Instance.FactoryManager;
        }

        private void Update()
        {
            if (!IsInitialized || _SpawnConfig == null || _SpawnConfig.Timer >= _SpawnConfig.Duration) return;

            _spawnTimer -= Time.deltaTime;
            _SpawnConfig.Timer += Time.deltaTime;

            if (_spawnTimer <= 0f)
            {
                SpawnEnemy();
                _spawnTimer = _SpawnConfig.Interval;
            }
        }

        public void Init(SpawnConfig spawnConfig, GameObject[] pathway = null)
        {
            _SpawnConfig = spawnConfig;
            Pathway = pathway;
            IsInitialized = true;
        }

        private void SpawnEnemy()
        {
            Enemy enemy = FactoryManager.GetEnemy(_SpawnConfig.EnemyType);
            enemy.SetPathway(Pathway);
            enemy.gameObject.transform.position = Pathway[0].transform.position;
        }
    }
}