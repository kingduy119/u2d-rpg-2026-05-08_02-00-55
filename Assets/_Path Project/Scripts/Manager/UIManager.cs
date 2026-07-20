using UnityEngine;

namespace TDGame
{
    public class UIManager : PersistentSingleton<UIManager>
    {
        [SerializeField] InGameUI m_InGameUI;
        [SerializeField] private GameObject missionCompletePanel;

        protected override void Awake()
        {
            base.Awake();
            // missionCompletePanel.SetActive(false);
        }

        private void OnEnable()
        {
            GameEvent.OnMissionComplete += HandleMissionComplete;
        }

        private void OnDisable()
        {
            GameEvent.OnMissionComplete -= HandleMissionComplete;
        }

        public void SetupUIMainMenu()
        {
            m_InGameUI.gameObject.SetActive(false);
            missionCompletePanel.SetActive(false);
        }

        public void SetupUIInGame()
        {
            m_InGameUI.gameObject.SetActive(true);
            missionCompletePanel.SetActive(false);
        }

        private void HandleMissionComplete()
        {
            missionCompletePanel.SetActive(true);
        }
    }
}