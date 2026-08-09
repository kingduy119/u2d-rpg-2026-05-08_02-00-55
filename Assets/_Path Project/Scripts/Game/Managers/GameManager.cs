using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : TDGame<GameManager>
    {

        public LevelManager LevelManager => Lazy.Load(ref _levelManager, _levelManagerPrefab, gameObject.transform);
        public TowerBoard TowerBoard => Lazy.Load(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        public GameStates GameStates;
        public FactoryManager FactoryManager;

        protected override void Awake()
        {
            base.Awake();
            GameStates = new();
            FactoryManager = new(this);
        }

        // private void Start()
        // {
        //     if (AudioController != null)
        //     {
        //         AudioController.PlayMainMenuMusic();
        //     }

        // }

        protected void OnEnable()
        {
            GameEvent.PlayNewGame += GameEvent_PlayNewGame;
            GameEvent.PlayContinue += GameEvent_PlayContinue;

            GameEvent.NavigateTo += NavigateTo;
            SceneManager.sceneLoaded += SceneManager_SceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_SceneUnloaded;
        }

        protected void OnDisable()
        {
            GameEvent.PlayNewGame -= GameEvent_PlayNewGame;
            GameEvent.PlayContinue -= GameEvent_PlayContinue;

            GameEvent.NavigateTo -= NavigateTo;
            SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
            SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
        }

        private void Update()
        {
            GameStates.Execute();
        }

        private void NavigateTo(string name)
        {
            SceneManager.LoadScene(name);
        }

        private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Debug.Log("SceneManager_SceneLoaded");
        }

        private void SceneManager_SceneUnloaded(Scene scene)
        {
            // Debug.Log("SceneManager_SceneUnloaded");
            FactoryManager.ResetObjects();
        }

        public void GameEvent_PlayNewGame(int level)
        {
            UIManager.Instance.InGameUI.gameObject.SetActive(true);
            GameStates.TransitionTo(GameStates.GamePlayState);
            LevelManager.LoadLevel(level);
        }

        public void GameEvent_PlayContinue()
        {
            UIManager.Instance.InGameUI.gameObject.SetActive(true);
            GameStates.TransitionTo(GameStates.GamePlayState);

            LevelManager.PlayContinueLevel();
        }
    }
}

