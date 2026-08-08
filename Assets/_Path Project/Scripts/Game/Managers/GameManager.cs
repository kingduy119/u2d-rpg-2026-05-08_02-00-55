using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : TDGame<GameManager>
    {

        public LevelManager LevelManager => Lazy.Load(ref _levelManager, _levelManagerPrefab, gameObject.transform);
        // public FactoryManager FactoryManager => Lazy.Load(ref _factoryManager, _factoryManagerPrefab, gameObject.transform);
        public TowerBoard TowerBoard => Lazy.Load(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        // public NewFactoryManager NewFactoryManager;
        public GameStates GameStates;
        // private PrefabManager prefabsManager;
        public FactoryManager FactoryManager;

        protected override void Awake()
        {
            base.Awake();
            GameStates = new();
            FactoryManager = new(this);
            // prefabsManager = new();

            // PrefabEvent.LoadTower += OnLoadTower;
        }

        private void Start()
        {
            if (AudioController != null)
            {
                AudioController.PlayMainMenuMusic();
            }

            // FactoryManager.LoadPrefabs();
            // prefabsManager.LoadPrefabs();
        }

        // private void OnLoadTower(Tower tower)
        // {
        //     Debug.Log($"tower.name: {tower.name}");
        // }

        protected void OnEnable()
        {
            GameEvent.PlayNewGame += GameEvent_PlayNewGame;
            GameEvent.PlayContinue += GameEvent_PlayContinue;

            GameEvent.NavigateTo += NavigateTo;
            // SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        }

        protected void OnDisable()
        {
            GameEvent.PlayNewGame -= GameEvent_PlayNewGame;
            GameEvent.PlayContinue -= GameEvent_PlayContinue;

            GameEvent.NavigateTo -= NavigateTo;
            // SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
        }

        private void Update()
        {
            GameStates.Execute();
        }

        private void NavigateTo(string name)
        {
            SceneManager.LoadScene(name);
        }

        // private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
        // {
        //     // if (FactoryManager != null)
        //     //     Destroy(FactoryManager.gameObject);
        // }

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

        // private void OnDestroy()
        // {
        //     // Debug.Log("GameManager.Destroy");
        //     // NewFactoryManager.Destroy();
        // }
    }
}

