using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private TowerSO[] _towers;
        [SerializeField] private FactoryManager _factoryManagerPrefab;
        [SerializeField] private SpawnManager _spawnManagerPrefab;

        private AudioController m_Audio;

        public InGameState InGameState;
        public AudioController Audio => m_Audio;
        public TowerSO[] Towers => _towers;

        private FactoryManager _factoryManager;
        public FactoryManager FactoryManager
        {
            get
            {
                if (_factoryManager == null) _factoryManager = Instantiate(_factoryManagerPrefab);
                return _factoryManager;
            }
        }

        private SpawnManager _spawnManager;
        public SpawnManager SpawnManager
        {
            get
            {
                if (_spawnManager == null) _spawnManager = Instantiate(_spawnManagerPrefab);
                return _spawnManager;
            }
        }



        protected override void Awake()
        {
            base.Awake();
            InGameState = new();

            var audio = transform.Find("AudioController");
            if (audio && audio.TryGetComponent<AudioController>(out var instance))
            {
                m_Audio = instance;
                m_Audio.PlayMainMenuMusic();
                GameEvent.Audio = m_Audio;
            }
        }

        private void OnEnable()
        {
            InGameState.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            InGameState.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("GetMouseButtonUp");
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InGameState.ResetOnLoadScene();

            if (FactoryManager != null)
                Destroy(FactoryManager.gameObject);

            GameEvent.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}

