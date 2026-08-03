using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    // public class GameManager : PersistentSingleton<GameManager>
    public class GameManager : TDGame<GameManager>
    {

        public LevelManager LevelManager => LazyLoad(ref _levelManager, _levelManagerPrefab, gameObject.transform);
        public FactoryManager FactoryManager => LazyLoad(ref _factoryManager, _factoryManagerPrefab, gameObject.transform);
        public TowerBoard TowerBoard => LazyLoad(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        public AudioController Audio => m_Audio;
        public GameState GameState;
        public GameStates GameStates;

        protected override void Awake()
        {
            base.Awake();
            GameState = new();
            GameStates = new();

            var audio = transform.Find("AudioController");
            if (audio && audio.TryGetComponent<AudioController>(out var instance))
            {
                m_Audio = instance;
                m_Audio.PlayMainMenuMusic();
                GameEvent.Audio = m_Audio;
            }
        }

        protected void OnEnable()
        {
            GameEvent.OnPlayNewGame += PlayNewGame;
            GameEvent.OnPlayContinue += PlayContinueGame;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected void OnDisable()
        {
            GameEvent.OnPlayNewGame -= PlayNewGame;
            GameEvent.OnPlayContinue -= PlayContinueGame;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Update()
        {
            GameStates.Execute();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // GameState.ResetOnLoadScene();

            if (FactoryManager != null)
                Destroy(FactoryManager.gameObject);

            // GameEvent.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void PlayNewGame(int level)
        {
            UIManager.Instance.InGameUI.gameObject.SetActive(true);
            GameStates.TransitionTo(GameStates.GamePlayState);

            LevelManager.LoadLevel(level);
        }

        public void PlayContinueGame()
        {
            UIManager.Instance.InGameUI.gameObject.SetActive(true);
            GameStates.TransitionTo(GameStates.GamePlayState);

            LevelManager.PlayContinueLevel();
        }

    }
}

