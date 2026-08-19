using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        [SerializeField] private bool Draw;


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
            GamePlayEvent.PathwayCount?.Invoke();
        }

        private void OnEnable()
        {
            GamePlayEvent.WaveStart += OnWaveStart;
        }

        private void OnDisable()
        {
            GamePlayEvent.WaveStart -= OnWaveStart;
        }

        public void OnWaveStart(int WaveNumber)
        {
            Debug.Log($"_spawnConfigMap.Count: {_spawnConfigMap.Count}");
            if (_spawnConfigMap.Count <= 0) return;

            if (_spawnConfigMap.TryGetValue(WaveNumber, out var spawnConfigs))
            {
                foreach (var spawnConfig in spawnConfigs)
                {
                    Spawner spawner = Instantiate(SpawnerPrefab);
                    spawner.Init(spawnConfig, Pathway);
                }

                _spawnConfigMap.Remove(WaveNumber);
                if (_spawnConfigMap.Count <= 0)
                {
                    GamePlayEvent.PathwayEnd?.Invoke();
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Draw) return;

            Gizmos.color = Color.red;
            for (int i = 0; i < Pathway.Length - 1; i++)
            {
                GUIStyle style = new();
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;

                Handles.Label(Pathway[i].transform.position, Pathway[i].name, style);
                Gizmos.DrawLine(Pathway[i].transform.position, Pathway[i + 1].transform.position);
            }
        }
#endif

    }
}
