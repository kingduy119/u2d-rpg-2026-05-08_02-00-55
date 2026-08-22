using UnityEngine.SceneManagement;

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

        private Price Price;

        private bool IsLoading;

        private readonly GameManager GM;
        readonly AssetLoader _GamePlayUILoader;

        public GamePlayState(GameManager gm)
        {
            GM = gm;
            _GamePlayUILoader = new("Game/GamePlayUI", true);
        }


        public void Enter()
        {
            GameEvent.LoadingSceneDone += OnLoadingSceneDone;
            SceneManager.sceneLoaded += OnSceneLoaded;

            GamePlayEvent.RequestUpdateUI += OnRequestUpdateUI;
            GamePlayEvent.ResponseLevelResource += OnResponseLevelResource;
            GamePlayEvent.MissionCompleteClick += OnMissionCompleteClick;
            GamePlayEvent.MainMenuClick += OnMainMenuClick;
            GamePlayEvent.WaveEnd += OnWaveEnd;
            GamePlayEvent.BuyTower += GamePlayEvent_BuyTower;

            EnemyEvent.EnemySpawn += EnemyEvent_EnemySpawn;
            EnemyEvent.EnemyDie += EnemyEvent_EnemyDie;
            EnemyEvent.ReachedEnd += EnemyEvent_ReachedEnd;
            EnemyEvent.ReceiveReward += EnemyEvent_ReceiveReward;

            GamePlayEvent.RequestLevelResource?.Invoke();
        }

        public void Exit()
        {
            GameEvent.LoadingSceneDone -= OnLoadingSceneDone;
            SceneManager.sceneLoaded -= OnSceneLoaded;

            GamePlayEvent.RequestUpdateUI -= OnRequestUpdateUI;
            GamePlayEvent.ResponseLevelResource -= OnResponseLevelResource;
            GamePlayEvent.MissionCompleteClick -= OnMissionCompleteClick;
            GamePlayEvent.MainMenuClick -= OnMainMenuClick;
            GamePlayEvent.WaveEnd -= OnWaveEnd;
            GamePlayEvent.BuyTower += GamePlayEvent_BuyTower;

            EnemyEvent.EnemySpawn -= EnemyEvent_EnemySpawn;
            EnemyEvent.EnemyDie -= EnemyEvent_EnemyDie;
            EnemyEvent.ReachedEnd -= EnemyEvent_ReachedEnd;
            EnemyEvent.ReceiveReward -= EnemyEvent_ReceiveReward;

            IsLoading = false;
        }

        public void Execute()
        {
            if (IsDirty)
            {
                GamePlayEvent.ResponseUpdateUI?.Invoke(this);
                IsDirty = false;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (IsLoading)
            {
                _GamePlayUILoader.Instantiate();
            }
        }
        public void OnLoadingSceneDone() => IsLoading = true;


        public void Destroy()
        {
            _GamePlayUILoader.Release();
        }


        private void OnMainMenuClick()
        {
            GM.StateMachine.TransitionTo(GM.MenuState);
            GameEvent.LoadScene?.Invoke("TD_MainMenu");
        }


        private void OnResponseLevelResource(LevelSO level)
        {
            Golds = level.startingGold;
            Lives = level.startingLives;
        }

        private void OnRequestUpdateUI() => GamePlayEvent.ResponseUpdateUI?.Invoke(this);

        private void OnMissionCompleteClick()
        {
            GM.StateMachine.TransitionTo(GM.MenuState);
            GameEvent.LoadScene?.Invoke("TD_MainMenu");
        }

        private void OnWaveEnd()
        {
            IsStarted = false;
            WaveCount++;
        }

        private void EnemyEvent_EnemySpawn() => Enemies++;
        private void EnemyEvent_ReachedEnd(Enemy enemy)
        {
            Enemies--;
            Lives -= enemy.SO.damage;
            if (Lives <= 0)
            {
                GamePlayEvent.GameOver?.Invoke();
            }
        }

        private void GamePlayEvent_BuyTower(Tower tower)
        {
            Price -= tower.SO.Price;
        }

        // public void ResetOnLoadScene()
        // {
        //     WaveCount = 0;
        // }

        public void EnemyEvent_EnemyDie(Enemy enemy)
        {
            Enemies--;
            Golds += enemy.SO.goldReward;
        }
        public void EnemyEvent_ReceiveReward(Enemy enemy) => Golds += enemy.SO.goldReward;

    }

}