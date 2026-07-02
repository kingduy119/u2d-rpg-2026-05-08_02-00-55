using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class UIManager : PersistentSingleton<UIManager>
    {
        [SerializeField] InGameUI m_InGameUI;
        [SerializeField] TowerSelectUI m_TowerSelectUI;

        protected override void Awake()
        {
            base.Awake();
        }

        // private void OnValidate()
        // {
        //     if (SceneManager.GetActiveScene().name != "TD_MainMenu")
        //     {
        //         SetupUIInGame();
        //     }
        // }

        public void SetupUIMainMenu()
        {
            m_InGameUI?.gameObject.SetActive(false);
            m_TowerSelectUI?.gameObject.SetActive(false);
        }

        public void SetupUIInGame()
        {
            m_InGameUI?.gameObject.SetActive(true);
            m_TowerSelectUI?.gameObject.SetActive(false);
        }
    }
}