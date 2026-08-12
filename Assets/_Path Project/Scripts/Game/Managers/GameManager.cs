using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : TDGame<GameManager>
    {

        public TowerBoard TowerBoard => Lazy.Load(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        public GameStates GameStates;
        public LevelManager LevelManager;
        public FactoryManager FactoryManager;

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            Coroutines.Initialize(this);

            GameStates = new(this);
            LevelManager = new();
            FactoryManager = new(this);
        }

        protected void OnEnable()
        {
            // GameEvent.NavigateTo += NavigateTo;
            SceneManager.sceneLoaded += SceneManager_SceneLoaded;
            SceneManager.sceneUnloaded += SceneManager_SceneUnloaded;
        }

        protected void OnDisable()
        {
            // GameEvent.NavigateTo -= NavigateTo;
            SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
            SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
        }

        private void Update()
        {
            GameStates.Execute();
        }

        // private void NavigateTo(string name)
        // {
        //     SceneManager.LoadScene(name);
        // }

        private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Debug.Log("SceneManager_SceneLoaded");
        }

        private void SceneManager_SceneUnloaded(Scene scene)
        {
            FactoryManager.ResetObjects();
        }
    }
}

