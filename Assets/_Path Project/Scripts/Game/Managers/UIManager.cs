using System;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.AddressableAssets;


namespace TDGame
{

    public class UIManager : PersistentSingleton<UIManager>
    {
        [SerializeField] private InGameUI _InGameUI;
        [SerializeField] private TowerAbilityOptionsUI _TowerAbilityOptionsPrefab;
        [SerializeField] private GameObject _MissionCompleteUIPrefab;

        private TowerAbilityOptionsUI _TowerAbilityOptionsUI;
        public TowerAbilityOptionsUI TowerAbilityOptionsUI => Lazy.Load(ref _TowerAbilityOptionsUI, _TowerAbilityOptionsPrefab, gameObject.transform);

        private GameObject _MissionCompleteUI;
        public GameObject MissionCompleteUI => Lazy.Load(ref _MissionCompleteUI, _MissionCompleteUIPrefab, gameObject.transform);


        [SerializeField] private GameObject _TowerPlaceCursorPrefab;
        private GameObject _TowerPlaceCursor;
        public GameObject TowerPlaceCursor => Lazy.Load(ref _TowerPlaceCursor, _TowerPlaceCursorPrefab, gameObject.transform);

        [SerializeField] private GameObject _TowerSelectCursorPrefab;
        private GameObject _TowerSelectCursor;
        public GameObject TowerSelectCursor => Lazy.Load(ref _TowerSelectCursor, _TowerSelectCursorPrefab, gameObject.transform);


        public InGameUI InGameUI => _InGameUI;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            InGameEvent.MissionComplete += MissionComplete;
            SceneManager.sceneUnloaded += SceneManager_SceneUnloaded;
        }

        private void OnDisable()
        {
            InGameEvent.MissionComplete -= MissionComplete;
            SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
        }

        public void SetupUIMainMenu()
        {
            _InGameUI.gameObject.SetActive(false);
        }

        public void SetupUIInGame()
        {
            _InGameUI.gameObject.SetActive(true);
        }

        private void MissionComplete() { MissionCompleteUI.SetActive(true); }

        private void SceneManager_SceneUnloaded(Scene scene)
        {
            if (MissionCompleteUI != null) Destroy(MissionCompleteUI);

        }
    }
}