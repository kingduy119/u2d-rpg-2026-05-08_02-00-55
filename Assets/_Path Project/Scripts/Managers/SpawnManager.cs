using UnityEngine;
using System;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance { get; set; }
    public static event Action<int> OnWaveChanged;

    public float _spawnTimer = 0f;
    public float _spawnInterval = 1f;
    private float _timeBetweenWaves = 3f;
    private float _waveCooldown = 3f;
    private bool _isWaveActive = false;

    private int _spawnedCount = 0;
    private int _enemiesRemoved = 0;
    private int _currentWaveIndex = 0;
    // public WaveData[] waves;
    private WaveData[] _waves;
    private WaveData CurrentWave => _waves[_currentWaveIndex];

    [SerializeField] private Object_Pool basicPool;
    [SerializeField] private Object_Pool normalPool;
    [SerializeField] private Object_Pool fastPool;
    [SerializeField] private Object_Pool mumyOrcPool;
    private Dictionary<TDEnemyType, Object_Pool> poolDictionary;

    public bool ActiveWave => _isWaveActive;

    void Awake()
    {
        poolDictionary = new Dictionary<TDEnemyType, Object_Pool>()
        {
            { TDEnemyType.MummyOrc, mumyOrcPool },
            { TDEnemyType.Basic, basicPool },
            { TDEnemyType.Normal, normalPool },
            { TDEnemyType.Fast, fastPool }
        };

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        TDEnemy.OnEnemyReachedEnd += HandlePointReachedEnd;
        TDEnemy.OnEnemyDestroyed += HandleEnemyDestroyed;
    }
    private void OnDisable()
    {
        TDEnemy.OnEnemyReachedEnd -= HandlePointReachedEnd;
        TDEnemy.OnEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Start()
    {
        _spawnTimer = _spawnInterval;
        _waves = LevelManager.Instance.CurrentLevel.waves;
        OnWaveChanged?.Invoke(_currentWaveIndex);
    }

    void Update()
    {
        if (!_isWaveActive) return;

        // if (_isWaveActive)
        // {
        //     _waveCooldown -= Time.deltaTime;
        //     if (_waveCooldown <= 0f)
        //     {
        //         _isWaveActive = false;
        //         _waveCooldown = _timeBetweenWaves;
        //     }
        // }
        // else
        // {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f && _spawnedCount < CurrentWave.perway)
        {
            _spawnTimer = _spawnInterval;
            SpawnObject();
        }
        else if (_enemiesRemoved >= CurrentWave.perway)
        {
            _currentWaveIndex = (_currentWaveIndex + 1) % _waves.Length;
            _spawnedCount = 0;
            _enemiesRemoved = 0;
            _isWaveActive = false;
            OnWaveChanged?.Invoke(_currentWaveIndex);
        }
        // }
    }

    public void StartNewWave() => _isWaveActive = true;

    private void SpawnObject()
    {
        GameObject obj = poolDictionary[CurrentWave.enemyType].GetObject();
        obj.transform.position = transform.position;
        obj.SetActive(true);
        _spawnedCount++;
    }


    private void HandlePointReachedEnd(TDEnemyData pointData)
    {
        _enemiesRemoved++;
    }

    private void HandleEnemyDestroyed(TDEnemy enemy)
    {
        _enemiesRemoved++;
    }
}
