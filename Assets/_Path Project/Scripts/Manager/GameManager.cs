using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private TowerSO[] _towers;
        [SerializeField] private FactoryManager _factoryPrefab;

        private AudioController m_Audio;

        public InGameState InGameState;
        public AudioController Audio => m_Audio;
        public TowerSO[] Towers => _towers;

        public FactoryManager m_factoryManager;
        public FactoryManager FactoryManager
        {
            get
            {
                if (m_factoryManager == null) m_factoryManager = Instantiate(_factoryPrefab);
                return m_factoryManager;
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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InGameState.ResetOnLoadScene();

            if (FactoryManager != null)
                Destroy(FactoryManager.gameObject);

            GameEvent.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}

