
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

        public GamePlayState() { }

        public void Enter()
        {
            GameEvent.OnEnemyDie += HandleEnemyDie;
            GameEvent.OnGetEnemyReward += HandleGetEnemyReward;

            InGameEvent.OnStartWave += StartWave;
            InGameEvent.OnEndWave += EndWave;
            InGameEvent.OnEnemyReachedEnd += EnemyReachedEnd;
            InGameEvent.OnEnemySpawn += EnemySpawn;
        }

        public void Exit()
        {
            GameEvent.OnEnemyDie -= HandleEnemyDie;
            GameEvent.OnGetEnemyReward -= HandleGetEnemyReward;

            InGameEvent.OnStartWave -= StartWave;
            InGameEvent.OnEndWave -= EndWave;
            InGameEvent.OnEnemyReachedEnd -= EnemyReachedEnd;
            InGameEvent.OnEnemySpawn -= EnemySpawn;
        }

        private void StartWave()
        {
            IsStarted = true;
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
            Lives -= enemy.Data.damage;
            if (Lives <= 0)
            {
                InGameEvent.OnGameOver?.Invoke();
            }
        }

        public void ResetOnLoadScene()
        {
            WaveCount = 0;
        }

        public void HandleEnemyDie(Enemy _) => Enemies--;
        public void HandleGetEnemyReward(Enemy enemy) => Golds += enemy.Data.goldReward;
    }
}