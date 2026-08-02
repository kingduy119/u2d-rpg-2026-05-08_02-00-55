using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : PersistentSingleton<GameManager>
    {
        [SerializeField] private LevelManager _levelManagerPrefab;
        [SerializeField] private FactoryManager _factoryManagerPrefab;
        [SerializeField] private TowerBoard _towerBoardPrefab;

        private AudioController m_Audio;
        private LevelManager _levelManager;
        private FactoryManager _factoryManager;
        private TowerBoard _towerBoard;

        public LevelManager LevelManager => LazyLoad(ref _levelManager, _levelManagerPrefab, gameObject.transform);
        public FactoryManager FactoryManager => LazyLoad(ref _factoryManager, _factoryManagerPrefab, gameObject.transform);
        public TowerBoard TowerBoard => LazyLoad(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        public AudioController Audio => m_Audio;
        public GameState GameState;

        protected override void Awake()
        {
            base.Awake();
            GameState = new();

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
            GameState.OnEnable();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            GameState.OnDisable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameState.ResetOnLoadScene();

            if (FactoryManager != null)
                Destroy(FactoryManager.gameObject);

            GameEvent.LoadScene(SceneManager.GetActiveScene().name);
        }

        private T LazyLoad<T>(ref T instance, T prefab, Transform transform = null)
            where T : Object
        {
            if (instance == null)
                instance = Instantiate(prefab, transform);

            return instance;
        }
    }
}

