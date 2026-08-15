using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections;

namespace TDGame
{

    public class GameManager : PersistentSingleton<GameManager>
    {

        [SerializeField] protected TowerBoard _towerBoardPrefab;
        [SerializeField] protected AudioController AudioController;

        protected FactoryManager _factoryManager;
        protected TowerBoard _towerBoard;

        public TowerBoard TowerBoard => Lazy.Load(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        public FactoryManager FactoryManager { get; private set; }
        public LevelManager LevelManager { get; private set; }

        public StateMachine StateMachine;
        private IState SetupState;
        public IState MenuState;
        public IState GamePlayState;

        public static string SceneName { get; private set; }
        AssetLoader LevelManagerLoader;


        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            Coroutines.Initialize(this);

            FactoryManager = new(this);
            LevelManagerLoader = new("Game/LevelManager", true);

            StateMachine = new();
            SetupState = new GameSetupState(this);
            MenuState = new GameMenuState(this);
            GamePlayState = new GamePlayState(this);
            StateMachine.Initialize(SetupState);
        }

        private IEnumerator Start()
        {
            while (!LevelManagerLoader.IsLoaded)
                yield return null;

            LevelManagerLoader.Instantiate(transform);
        }

        protected void OnEnable()
        {
            GameEvent.LoadScene += OnLoadScene;
        }

        protected void OnDisable()
        {
            GameEvent.LoadScene -= OnLoadScene;
        }


        private void Update()
        {
            StateMachine.Execute();
        }

        private void OnLoadScene(string name)
        {
            SceneName = name;
            Addressables.LoadSceneAsync("LoadingScene", activateOnLoad: true);
        }
    }
}

