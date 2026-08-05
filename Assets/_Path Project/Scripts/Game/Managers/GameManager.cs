using System.Threading.Tasks;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : TDGame<GameManager>
    {

        public LevelManager LevelManager => LazyLoad(ref _levelManager, _levelManagerPrefab, gameObject.transform);
        public FactoryManager FactoryManager => LazyLoad(ref _factoryManager, _factoryManagerPrefab, gameObject.transform);
        public TowerBoard TowerBoard => LazyLoad(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        // public GameState GameState;
        public GameStates GameStates;

        public static IAssetLoader AssetLoader { get; private set; }

        // private LoadingScreen _LoadingScreen;
        public LoadingScreen LoadingScreen;
        // {
        //     get
        //     {
        //         TestLoad();
        //         return _LoadingScreen;
        //     }
        // }

        protected override void Awake()
        {
            base.Awake();
            // GameState = new();
            GameStates = new();
            AssetLoader = new AddressableLoader();
        }

        private async void Start()
        {
            if (AudioController != null)
            {
                AudioController.PlayMainMenuMusic();
            }
            TestLoad();
            // GameObject loading = await AssetLoader.InstantiateAsync("Game/LoadingUI");
            // LoadingScreen = loading.GetComponent<LoadingScreen>();
            // LoadingScreen.gameObject.SetActive(false);
        }

        private async void TestLoad()
        {
            GameObject loading = await AssetLoader.InstantiateAsync("Game/LoadingUI");
            LoadingScreen = loading.GetComponent<LoadingScreen>();
            LoadingScreen.gameObject.SetActive(false);
        }

        protected void OnEnable()
        {
            GameEvent.PlayNewGame += GameEvent_PlayNewGame;
            GameEvent.PlayContinue += GameEvent_PlayContinue;

            GameEvent.NavigateTo += NavigateTo;
            SceneManager.sceneLoaded += SceneManager_SceneLoaded;
        }

        protected void OnDisable()
        {
            GameEvent.PlayNewGame -= GameEvent_PlayNewGame;
            GameEvent.PlayContinue -= GameEvent_PlayContinue;

            GameEvent.NavigateTo -= NavigateTo;
            SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
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
            if (FactoryManager != null)
                Destroy(FactoryManager.gameObject);
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

