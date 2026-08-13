using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
// using UnityEngine.AddressableAssets;


namespace TDGame
{
    public enum Game
    {
        GamePlayUI
    }

    public class UIManager : PersistentSingleton<UIManager>
    {
        // private string[] _Keys = [Game.GamePlayUI.ToString()];
        // [SerializeField] private GamePlayUI _GamePlayUI;
        [SerializeField] private TowerAbilityOptionsUI _TowerAbilityOptionsPrefab;
        // [SerializeField] private GameObject _MissionCompleteUIPrefab;

        // private TowerAbilityOptionsUI _TowerAbilityOptionsUI;
        // public TowerAbilityOptionsUI TowerAbilityOptionsUI => Lazy.Load(ref _TowerAbilityOptionsUI, _TowerAbilityOptionsPrefab, gameObject.transform);

        // private GameObject _MissionCompleteUI;
        // public GameObject MissionCompleteUI => Lazy.Load(ref _MissionCompleteUI, _MissionCompleteUIPrefab, gameObject.transform);


        [SerializeField] private GameObject _TowerPlaceCursorPrefab;
        private GameObject _TowerPlaceCursor;
        public GameObject TowerPlaceCursor => Lazy.Load(ref _TowerPlaceCursor, _TowerPlaceCursorPrefab, transform);

        [SerializeField] private GameObject _TowerSelectCursorPrefab;
        private GameObject _TowerSelectCursor;
        public GameObject TowerSelectCursor => Lazy.Load(ref _TowerSelectCursor, _TowerSelectCursorPrefab, transform);


        // public GamePlayUI GamePlayUI => _GamePlayUI;

        private readonly List<AsyncOperationHandle<GameObject>> _handles = new();
        private readonly List<GameObject> _objects = new();

        private GamePlayUI _GamePlayUI;


        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            Coroutines.StartCoroutine(LoadAssets());
        }

        private IEnumerator LoadAssets()
        {
            string[] keys =
            {
                // "Game/GamePlayUI",
                // "Game/SelectTowerCursorUI",
                // "Game/PlaceTowerCursorUI"
                // "Game/TowerPlaceCursorUI",
            };

            foreach (var key in keys)
            {
                var handle = Addressables.InstantiateAsync(key);
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    // Debug.Log($"UIManager: {handle.Result.name}");
                    var go = handle.Result;
                    if (go.TryGetComponent<GamePlayUI>(out var GamePlayUI))
                    {
                        _GamePlayUI = GamePlayUI;
                    }
                    go.SetActive(false);
                    _objects.Add(go);
                }
                _handles.Add(handle);
                // _handles.Add(Addressables.InstantiateAsync(key));
            }

            // foreach (var handle in _handles)
            // {
            //     yield return handle;

            //     if (handle.Status == AsyncOperationStatus.Succeeded)
            //     {
            //         _objects.Add(handle.Result);
            //     }
            // }
        }

        private void OnEnable()
        {
            // GamePlayEvent.MissionComplete += MissionComplete;
            // SceneManager.sceneUnloaded += SceneManager_SceneUnloaded;
        }

        private void OnDisable()
        {
            // GamePlayEvent.MissionComplete -= MissionComplete;
            // SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
        }

        // public void SetupUIMainMenu()
        // {
        //     _GamePlayUI.gameObject.SetActive(false);
        // }

        // public void SetupUIInGame()
        // {
        //     _GamePlayUI.gameObject.SetActive(true);
        // }

        // private void MissionComplete() { MissionCompleteUI.SetActive(true); }

        // private void SceneManager_SceneUnloaded(Scene scene)
        // {
        //     if (MissionCompleteUI != null) Destroy(MissionCompleteUI);

        // }
    }
}