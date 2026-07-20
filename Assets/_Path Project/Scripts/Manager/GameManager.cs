using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private TowerSO[] m_towers;

        private AudioController m_Audio;

        public InGameState InGameState;
        public AudioController Audio => m_Audio;
        public TowerSO[] Towers => m_towers;

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
            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => GameEvent.LoadScene(SceneManager.GetActiveScene().name);
    }

}

