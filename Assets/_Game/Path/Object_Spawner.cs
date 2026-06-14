using System.Collections.Generic;
using UnityEngine;

public class Object_Spawner : MonoBehaviour
{
    public float _spawnTimer;
    public float _spawnInterval = 1f;
    private int _currentWaveIndex = 0;
    private int _spawnedCount = 0;
    private int _waveEndCount = 0;
    private WaveData CurrentWave => waves[_currentWaveIndex];

    // public GameObject _prefab;
    public WaveData[] waves;
    [SerializeField] private Object_Pool basicPool;
    [SerializeField] private Object_Pool normalPool;
    [SerializeField] private Object_Pool fastPool;
    // [SerializeField] private Object_Pool tankPool;
    // [SerializeField] private Object_Pool bossPool;
    private Dictionary<PointType, Object_Pool> poolDictionary;


    void Awake()
    {
        poolDictionary = new Dictionary<PointType, Object_Pool>()
        {
            { PointType.Basic, basicPool },
            { PointType.Normal, normalPool },
            { PointType.Fast, fastPool }
            // { PointType.Tank, tankPool },
            // { PointType.Boss, bossPool }
        };

    }

    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f && _spawnedCount < CurrentWave.perway)
        {
            _spawnTimer = _spawnInterval;
            SpawnObject();
        }
        else if (_spawnedCount >= CurrentWave.perway)
        {
            _currentWaveIndex = (_currentWaveIndex + 1) % waves.Length;
            _spawnedCount = 0;
        }
    }

    private void SpawnObject()
    {
        GameObject obj = poolDictionary[CurrentWave.pointType].GetObject();
        obj.transform.position = transform.position;
        obj.SetActive(true);
        _spawnedCount++;
    }

    private void OnEnable()
    {
        Point.OnPointReachedEnd += HandlePointReachedEnd;
    }
    private void OnDisable()
    {
        Point.OnPointReachedEnd -= HandlePointReachedEnd;
    }

    private void HandlePointReachedEnd(PointData pointData)
    {
        // Handle the event when a point reaches the end
        _waveEndCount++;
    }
}
