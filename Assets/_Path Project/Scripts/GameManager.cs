using System;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.InputSystem;

namespace TDGame
{
    public class GameManager : PersistentSingleton<GameManager>
    {

        [SerializeField] private TowerSO[] m_towers;
        public TowerSO[] Towers => m_towers;

        public InGameController InGame { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InGame = new InGameController();
            AudioManager.Instance.PlayMainMenuMusic();
        }

        void OnEnable()
        {
            // Enemy.OnEnemyReachedEnd += InGame.HandlePointReachedEnd;
            // Enemy.OnGetEnemyReward += InGame.HandleGetEnemyReward;

            InGame.RegisterEvent();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            // Enemy.OnEnemyReachedEnd -= InGame.HandlePointReachedEnd;
            // Enemy.OnGetEnemyReward -= InGame.HandleGetEnemyReward;
            InGame.UnregisterEvent();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Start()
        {
            LoadScene();
        }

        private void Update()
        {
            InGame.Update();
        }

        // private void Update()
        // {
        //     if (Mouse.current.leftButton.wasPressedThisFrame)
        //     {
        //         Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //         RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        //         if (hit.collider == null) return;

        //         Debug.Log(hit.collider.gameObject.name);
        //         Debug.Log(LayerMask.LayerToName(hit.collider.gameObject.layer));

        //         switch (LayerMask.LayerToName(hit.collider.gameObject.layer))
        //         {
        //             case "Tile":
        //                 Debug.Log("Click Tile");
        //                 break;

        //             case "Tower":
        //                 Debug.Log("Click Tower");
        //                 break;

        //             case "Enemy":
        //                 Debug.Log("Click Enemy");
        //                 break;
        //         }
        //     }
        // }

        // private void HandlePointReachedEnd(EnemyData enemy)
        // {
        //     Lives -= enemy.damage;
        //     if (Lives <= 0)
        //     {
        //         Debug.Log("Game Over!");
        //     }
        // }

        // private void HandleGetEnemyReward(EnemyData enemy) => Golds += enemy.goldReward;
        // public void SetTimeScale(float scale) => Time.timeScale = scale;

        public void SpendGold(int amount)
        {
            // if (Golds >= amount)
            //     Golds -= amount;
            // else
            // {
            //     Debug.LogWarning("Not enough gold!");
            // }
        }

        private void LoadScene() => GameEvent.LoadScene(SceneManager.GetActiveScene().name);
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) => GameEvent.LoadScene(SceneManager.GetActiveScene().name);

    }

}