using System;
using UnityEngine;

namespace TDGame
{
    public class UIManager : PersistentSingleton<UIManager>
    {
        [SerializeField] private InGameUI _InGameUI;
        // [SerializeField] private GameObject _missionCompletePanel;
        [SerializeField] private TowerAbilityOptionsUI _TowerAbilityOptionsPrefab;
        [SerializeField] private MissionCompleteUI _MissionCompleteUIPrefab;
        // [SerializeField] private TowerSelectCursor _TowerSelectCursorPrefab;

        private TowerAbilityOptionsUI _TowerAbilityOptionsUI;
        public TowerAbilityOptionsUI TowerAbilityOptionsUI => LazyLoad(ref _TowerAbilityOptionsUI, _TowerAbilityOptionsPrefab, gameObject.transform);

        private MissionCompleteUI _MissionCompleteUI;
        public MissionCompleteUI MissionCompleteUI => LazyLoad(ref _MissionCompleteUI, _MissionCompleteUIPrefab, gameObject.transform);

        // private TowerSelectCursor _TowerSelectCursor;
        // public TowerSelectCursor TowerSelectCursor => LazyLoad(ref _TowerSelectCursor, _TowerSelectCursorPrefab, gameObject.transform);

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            GameEvent.OnMissionComplete += HandleMissionComplete;
            // TowerEvent.OnSelectUpdateTower += ShowTowerUpdateSelect;
            // TowerEvent.OnAbilitySelect += CloseTowerUpdateSelect;
        }

        private void OnDisable()
        {
            GameEvent.OnMissionComplete -= HandleMissionComplete;
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
            MissionCompleteUI.gameObject.SetActive(true);
        }

        // public void ShowTowerUpdateSelect()
        // {
        //     TowerAbilityOptionsUI.gameObject.SetActive(true);
        // }
        // public void CloseTowerUpdateSelect(Ability _ = null)
        // {
        //     TowerAbilityOptionsUI.gameObject.SetActive(false);
        // }

        private T LazyLoad<T>(ref T instance, T prefab, Transform transform = null) where T : MonoBehaviour
        {
            if (instance == null)
                instance = Instantiate(prefab, transform);

            return instance;
        }
    }
}