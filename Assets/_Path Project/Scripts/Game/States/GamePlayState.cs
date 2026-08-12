


using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{
    public class GamePlayState :
        DirtyState,
        IState
    {
        private bool _isStarted = false;
        public bool IsStarted
        {
            get => _isStarted;
            set => SetValue(ref _isStarted, value);
        }

        private int _lives = 0;
        public int Lives
        {
            get => _lives;
            set => SetValue(ref _lives, value);
        }

        private int _golds = 0;
        public int Golds
        {
            get => _golds;
            set => SetValue(ref _golds, value);
        }

        private int _rocks = 0;
        public int Rocks
        {
            get => _rocks;
            set => SetValue(ref _rocks, value);
        }

        private int _wood = 0;
        public int Woods
        {
            get => _wood;
            set => SetValue(ref _wood, value);
        }

        private int _wave = 0;
        public int WaveCount
        {
            get => _wave;
            set => SetValue(ref _wave, value);
        }

        private int _enemies = 0;
        public int Enemies
        {
            get => _enemies;
            set => SetValue(ref _enemies, value);
        }

        // Assets:
        // private GameObject _GamePlayUI;
        // private AsyncOperationHandle<GameObject> _handle;
        private GameManager _GameManager;

        public GamePlayState(GameManager gm)
        {
            _GameManager = gm;
            // Coroutines.StartCoroutine(LoadAsset());
        }

        // IEnumerator LoadAsset()
        // {
        //     _handle = Addressables.InstantiateAsync("Game/GamePlayUI");
        //     yield return _handle;

        //     if (_handle.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         _GamePlayUI = _handle.Result;
        //         _GamePlayUI.SetActive(false);
        //     }
        // }

        // public void Destroy()
        // {
        //     Addressables.Release(_handle);
        // }

        public void Enter()
        {
            // if (_GamePlayUI != null) _GamePlayUI.SetActive(true);

            // InGameEvent.StartWave += StartWave;
            InGameEvent.OnEndWave += EndWave;
            InGameEvent.OnLevelLoaded += LoadLevelResource;
            InGameEvent.MissionCompleteClick += OnMissionCompleteClick;

            EnemyEvent.OnEnemyDie += HandleEnemyDie;
            EnemyEvent.OnGetEnemyReward += HandleGetEnemyReward;
            EnemyEvent.OnEnemyReachedEnd += EnemyReachedEnd;

            EnemyEvent.EnemySpawn += EnemySpawn;
        }

        public void Exit()
        {
            // if (_GamePlayUI != null) _GamePlayUI.SetActive(false);

            // InGameEvent.StartWave -= StartWave;
            InGameEvent.OnEndWave -= EndWave;
            InGameEvent.OnLevelLoaded -= LoadLevelResource;
            InGameEvent.MissionCompleteClick -= OnMissionCompleteClick;

            EnemyEvent.OnEnemyDie -= HandleEnemyDie;
            EnemyEvent.OnGetEnemyReward -= HandleGetEnemyReward;
            EnemyEvent.OnEnemyReachedEnd -= EnemyReachedEnd;

            EnemyEvent.EnemySpawn -= EnemySpawn;
        }

        private void OnMissionCompleteClick()
        {
            _GameManager.GameStates.TransitionTo(_GameManager.GameStates.GameMenuState);
        }

        public void Execute()
        {
            if (IsDirty)
            {
                InGameEvent.OnUpdateUI?.Invoke(this);
                IsDirty = false;
            }
        }

        private void EndWave()
        {
            IsStarted = false;
            WaveCount++;
        }

        private void LoadLevelResource(LevelSO level)
        {
            Golds = level.startingGold;
            Lives = level.startingLives;
        }

        private void EnemySpawn() => Enemies++;
        public void EnemyReachedEnd(Enemy enemy)
        {
            Enemies--;
            Lives -= enemy.SO.damage;
            if (Lives <= 0)
            {
                InGameEvent.GameOver?.Invoke();
            }
        }

        public void ResetOnLoadScene()
        {
            WaveCount = 0;
        }

        public void HandleEnemyDie(Enemy _) => Enemies--;
        public void HandleGetEnemyReward(Enemy enemy) => Golds += enemy.SO.goldReward;
    }
}