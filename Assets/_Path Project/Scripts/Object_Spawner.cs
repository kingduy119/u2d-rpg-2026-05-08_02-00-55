using UnityEngine;
using System;
using System.Collections.Generic;

public class Object_Spawner : MonoBehaviour
{
    public static event Action<int> OnWaveChanged;

    public float _spawnTimer;
    public float _spawnInterval = 1f;
    private float _timeBetweenWaves = 3f;
    private float _waveCooldown = 3f;
    private bool _isWaveActive = false;

    private int _currentWaveIndex = 0;
    private int _spawnedCount = 0;
    private int _waveEndCount = 0;
    private WaveData CurrentWave => waves[_currentWaveIndex];

    // public GameObject _prefab;
    public WaveData[] waves;
    [SerializeField] private Object_Pool basicPool;
    [SerializeField] private Object_Pool normalPool;
    [SerializeField] private Object_Pool fastPool;
    private Dictionary<PointType, Object_Pool> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<PointType, Object_Pool>()
        {
            { PointType.Basic, basicPool },
            { PointType.Normal, normalPool },
            { PointType.Fast, fastPool }
        };

    }

    private void OnEnable()
    {
        Point.OnPointReachedEnd += HandlePointReachedEnd;
    }
    private void OnDisable()
    {
        Point.OnPointReachedEnd -= HandlePointReachedEnd;
    }

    private void Start()
    {
        _spawnTimer = _spawnInterval;
        OnWaveChanged?.Invoke(_currentWaveIndex);
    }

    void Update()
    {
        if (_isWaveActive)
        {
            _waveCooldown -= Time.deltaTime;
            if (_waveCooldown <= 0f)
            {
                _isWaveActive = false;
                // _currentWaveIndex = 0;
                _waveCooldown = _timeBetweenWaves;
            }
        }
        else
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
                _isWaveActive = true;
            }
        }
    }


    private void SpawnObject()
    {
        GameObject obj = poolDictionary[CurrentWave.pointType].GetObject();
        obj.transform.position = transform.position;
        obj.SetActive(true);
        _spawnedCount++;
    }



    private void HandlePointReachedEnd(PointData pointData)
    {
        // Handle the event when a point reaches the end
        _waveEndCount++;
    }
}
