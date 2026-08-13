using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{
    public static class Lazy
    {
        public static T Load<T>(ref T instance, T prefab, Transform parent = null)
            where T : Object
        {
            if (instance == null)
            {
                instance = Object.Instantiate(prefab, parent);
            }

            return instance;
        }
    }

    public class GameManager : PersistentSingleton<GameManager>
    {

        [SerializeField] protected TowerBoard _towerBoardPrefab;
        [SerializeField] protected AudioController AudioController;

        protected FactoryManager _factoryManager;
        protected TowerBoard _towerBoard;

        public TowerBoard TowerBoard => Lazy.Load(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        public GameStates GameStates { get; private set; }
        public FactoryManager FactoryManager { get; private set; }
        public LevelManager LevelManager { get; private set; }

        public static string SceneName;


        // 
        private readonly List<AsyncOperationHandle<GameObject>> _handles = new();
        private readonly List<GameObject> _objects = new();

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Start()
        {
            Coroutines.StartCoroutine(LoadAssets());
        }

        private IEnumerator LoadAssets()
        {
            string[] keys =
            {
                "Game/LevelManager",
            };

            foreach (var key in keys)
            {
                var handle = Addressables.InstantiateAsync(key, transform);
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var go = handle.Result;
                    if (go.TryGetComponent<LevelManager>(out var levelManager))
                    {
                        LevelManager = levelManager;
                    }
                    _objects.Add(go);
                }
                _handles.Add(handle);
            }
        }

        private void Initialize()
        {
            Coroutines.Initialize(this);

            GameStates = new(this);
            FactoryManager = new(this);
        }

        protected void OnEnable()
        {
            GameEvent.LoadScene += OnLoadScene;
            //     // SceneManager.sceneLoaded += SceneManager_SceneLoaded;
            //     // SceneManager.sceneUnloaded += SceneManager_SceneUnloaded;
            // Game
        }

        protected void OnDisable()
        {
            GameEvent.LoadScene -= OnLoadScene;
            //     SceneManager.sceneLoaded -= SceneManager_SceneLoaded;
            //     SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
        }

        private void Update()
        {
            GameStates.Execute();
        }

        private void OnLoadScene(string name)
        {
            SceneName = name;
            Addressables.LoadSceneAsync("LoadingScene", activateOnLoad: true);
        }


        private void ReleaseAssets()
        {
            foreach (var go in _objects)
            {
                if (go != null)
                {
                    Addressables.ReleaseInstance(go);
                }
            }

            _objects.Clear();
            _handles.Clear();

            LevelManager = null;
        }

        private void OnDestroy()
        {
            ReleaseAssets();
        }
        // private void SceneManager_SceneLoaded(Scene scene, LoadSceneMode mode)
        // {
        //     // Debug.Log("SceneManager_SceneLoaded");
        // }

        // private void SceneManager_SceneUnloaded(Scene scene)
        // {
        //     FactoryManager.ResetObjects();
        // }
    }
}

