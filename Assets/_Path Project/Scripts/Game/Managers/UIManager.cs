using System;
using UnityEngine;

namespace TDGame
{

    public class UIManager : PersistentSingleton<UIManager>
    {
        [SerializeField] private InGameUI _InGameUI;
        [SerializeField] private TowerAbilityOptionsUI _TowerAbilityOptionsPrefab;
        [SerializeField] private GameObject _MissionCompleteUIPrefab;

        private TowerAbilityOptionsUI _TowerAbilityOptionsUI;
        public TowerAbilityOptionsUI TowerAbilityOptionsUI => LazyLoad(ref _TowerAbilityOptionsUI, _TowerAbilityOptionsPrefab, gameObject.transform);

        private GameObject _MissionCompleteUI;
        public GameObject MissionCompleteUI => LazyLoad(ref _MissionCompleteUI, _MissionCompleteUIPrefab, gameObject.transform);


        [SerializeField] private GameObject _TowerPlaceCursorPrefab;
        private GameObject _TowerPlaceCursor;
        public GameObject TowerPlaceCursor => LazyLoad(ref _TowerPlaceCursor, _TowerPlaceCursorPrefab, gameObject.transform);

        [SerializeField] private GameObject _TowerSelectCursorPrefab;
        private GameObject _TowerSelectCursor;
        public GameObject TowerSelectCursor => LazyLoad(ref _TowerSelectCursor, _TowerSelectCursorPrefab, gameObject.transform);


        public InGameUI InGameUI => _InGameUI;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            InGameEvent.OnMissionComplete += HandleMissionComplete;
            // TowerEvent.OnSelectUpdateTower += ShowTowerUpdateSelect;
            // TowerEvent.OnAbilitySelect += CloseTowerUpdateSelect;
        }

        private void OnDisable()
        {
            InGameEvent.OnMissionComplete -= HandleMissionComplete;
            // TowerEvent.OnSelectUpdateTower -= ShowTowerUpdateSelect;
            // TowerEvent.OnAbilitySelect -= CloseTowerUpdateSelect;
        }

        public void SetupUIMainMenu()
        {
            _InGameUI.gameObject.SetActive(false);
        }

        public void SetupUIInGame()
        {
            _InGameUI.gameObject.SetActive(true);
        }

        private void HandleMissionComplete()
        {
            MissionCompleteUI.SetActive(true);
        }

        // public void ShowTowerUpdateSelect()
        // {
        //     TowerAbilityOptionsUI.gameObject.SetActive(true);
        // }
        // public void CloseTowerUpdateSelect(Ability _ = null)
        // {
        //     TowerAbilityOptionsUI.gameObject.SetActive(false);
        // }

        private T LazyLoad<T>(ref T instance, T prefab, Transform transform = null)
            where T : UnityEngine.Object
        {
            if (instance == null)
                instance = Instantiate(prefab, transform);

            return instance;
        }
    }
}