using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace TDGame
{
    public class PathwaySpawner : MonoBehaviour
    {
        [Serializable]
        public class Config
        {
            public int WaveNumber;
            public SpawnConfig[] SpawnConfigs;
        }

        [SerializeField] private Spawner SpawnerPrefab;


        public Config[] configs;
        public GameObject[] Pathway;
        private Dictionary<int, SpawnConfig[]> _spawnConfigMap = new();

        private void Awake()
        {
            foreach (var config in configs)
            {
                _spawnConfigMap[config.WaveNumber] = config.SpawnConfigs;
            }
        }

        private void Start()
        {
            InGameEvent.PathwayStart?.Invoke();
        }

        private void OnEnable()
        {
            InGameEvent.StartWave += HandleStartWave;
        }

        private void OnDisable()
        {
            InGameEvent.StartWave -= HandleStartWave;
        }

        public void HandleStartWave(int WaveNumber)
        {
            if (_spawnConfigMap.Count <= 0) return;

            if (_spawnConfigMap.TryGetValue(WaveNumber, out var spawnConfigs))
            {
                foreach (var spawnConfig in spawnConfigs)
                {
                    Spawner spawnerInstance = Instantiate(SpawnerPrefab);
                    spawnerInstance.Init(spawnConfig, Pathway);
                }

                if (_spawnConfigMap.Remove(WaveNumber) && _spawnConfigMap.Count == 0)
                {
                    InGameEvent.PathwayEnd?.Invoke();
                }
            }
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     for (int i = 0; i < Pathway.Length - 1; i++)
        //     {
        //         GUIStyle style = new();
        //         style.normal.textColor = Color.white;
        //         style.alignment = TextAnchor.MiddleCenter;

        //         Handles.Label(Pathway[i].transform.position, Pathway[i].name, style);
        //         Gizmos.DrawLine(Pathway[i].transform.position, Pathway[i + 1].transform.position);
        //     }
        // }

    }
}
