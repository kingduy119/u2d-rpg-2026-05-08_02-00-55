using System;
using UnityEngine;

namespace TDGame
{
    public class UIManager : PersistentSingleton<UIManager>
    {
        [SerializeField] private InGameUI _InGameUI;
        [SerializeField] private GameObject _missionCompletePanel;
        [SerializeField] private GameObject _towerSkillsSelect;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            GameEvent.OnMissionComplete += HandleMissionComplete;
            TowerEvent.OnTowerUpdateSelect += ShowTowerUpdateSelect;
            TowerEvent.OnAbilitySelect += CloseTowerUpdateSelect;
        }

        private void OnDisable()
        {
            GameEvent.OnMissionComplete -= HandleMissionComplete;
            TowerEvent.OnTowerUpdateSelect -= ShowTowerUpdateSelect;
            TowerEvent.OnAbilitySelect -= CloseTowerUpdateSelect;
        }

        public void SetupUIMainMenu()
        {
            _InGameUI.gameObject.SetActive(false);
            _missionCompletePanel.SetActive(false);
            _towerSkillsSelect.SetActive(false);
        }

        public void SetupUIInGame()
        {
            _InGameUI.gameObject.SetActive(true);
            _missionCompletePanel.SetActive(false);
        }

        private void HandleMissionComplete()
        {
            _missionCompletePanel.SetActive(true);
        }

        public void ShowTowerUpdateSelect()
        {
            _towerSkillsSelect.SetActive(true);
        }
        public void CloseTowerUpdateSelect(Ability _ = null)
        {
            _towerSkillsSelect.SetActive(false);
        }
    }
}