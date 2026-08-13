


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
        private GameObject _GamePlayUI;
        private AsyncOperationHandle<GameObject> _handle;
        private readonly GameManager GM;

        public GamePlayState(GameManager gm)
        {
            GM = gm;
        }



        public void Active()
        {
            if (_GamePlayUI == null)
                Coroutines.StartCoroutine(LoadAsset());

            if (_GamePlayUI != null)
            {
                _GamePlayUI.SetActive(true);
            }
        }

        IEnumerator LoadAsset()
        {
            _handle = Addressables.InstantiateAsync("Game/GamePlayUI");
            yield return _handle;

            if (_handle.Status == AsyncOperationStatus.Succeeded)
            {
                _GamePlayUI = _handle.Result;
            }
        }

        public void OnLoadingDone() => Active();
        public void Deactivate()
        {
            if (_GamePlayUI != null) _GamePlayUI.SetActive(false);
        }

        public void Destroy()
        {
            Addressables.Release(_handle);
        }

        public void Enter()
        {
            GameEvent.LoadingDone += OnLoadingDone;
            GamePlayEvent.RequestUpdateUI += OnRequestUpdateUI;
            GamePlayEvent.ResponseLevelResource += OnResponseLevelResource;

            GamePlayEvent.OnEndWave += EndWave;
            GamePlayEvent.MissionCompleteClick += OnMissionCompleteClick;

            EnemyEvent.OnEnemyDie += HandleEnemyDie;
            EnemyEvent.OnGetEnemyReward += HandleGetEnemyReward;
            EnemyEvent.OnEnemyReachedEnd += EnemyReachedEnd;

            EnemyEvent.EnemySpawn += EnemySpawn;


            GamePlayEvent.RequestLevelResource?.Invoke();
        }

        public void Execute()
        {
            if (IsDirty)
            {
                GamePlayEvent.ResponseUpdateUI?.Invoke(this);
                IsDirty = false;
            }
        }

        public void Exit()
        {
            GameEvent.LoadingDone -= OnLoadingDone;
            GamePlayEvent.RequestUpdateUI -= OnRequestUpdateUI;
            GamePlayEvent.ResponseLevelResource -= OnResponseLevelResource;

            GamePlayEvent.OnEndWave -= EndWave;
            GamePlayEvent.MissionCompleteClick -= OnMissionCompleteClick;

            EnemyEvent.OnEnemyDie -= HandleEnemyDie;
            EnemyEvent.OnGetEnemyReward -= HandleGetEnemyReward;
            EnemyEvent.OnEnemyReachedEnd -= EnemyReachedEnd;

            EnemyEvent.EnemySpawn -= EnemySpawn;


            Deactivate();
        }

        private void OnResponseLevelResource(LevelSO level)
        {
            Golds = level.startingGold;
            Lives = level.startingLives;
        }

        private void OnRequestUpdateUI() => GamePlayEvent.ResponseUpdateUI?.Invoke(this);

        private void OnMissionCompleteClick()
        {
            GM.GameStates.TransitionTo(GM.GameStates.GameMenuState);
        }

        private void EndWave()
        {
            IsStarted = false;
            WaveCount++;
        }

        private void EnemySpawn() => Enemies++;
        public void EnemyReachedEnd(Enemy enemy)
        {
            Enemies--;
            Lives -= enemy.SO.damage;
            if (Lives <= 0)
            {
                GamePlayEvent.GameOver?.Invoke();
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