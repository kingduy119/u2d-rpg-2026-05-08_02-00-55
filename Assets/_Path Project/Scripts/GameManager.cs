using UnityEngine;
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameManager : PersistentSingleton<GameManager>
    {

        [SerializeField] private TowerSO[] m_towers;

        public TowerSO[] Towers => m_towers;
        public InGameState InGameState { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            AudioManager.Instance.PlayMainMenuMusic();
            InGameState = new();
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

        }

        void Start()
        {
            LoadScene();
        }

        private void LoadScene() => GameEvent.LoadScene(SceneManager.GetActiveScene().name);
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => GameEvent.LoadScene(SceneManager.GetActiveScene().name);


        public bool CheckAndSpendResource(TowerSO towerData)
        {
            if (InGameState.Golds >= towerData.cost)
            {
                InGameState.Golds -= towerData.cost;
                return true;
            }
            return false;
        }
    }

}

