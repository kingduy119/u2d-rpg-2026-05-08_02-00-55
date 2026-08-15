using UnityEngine;

namespace TDGame
{
    public class LevelManager : MonoBehaviour
    {

        public LevelSO[] AllLevels;
        public LevelState LevelState;

        private void Awake()
        {
            LevelState = new(AllLevels.Length);
        }

        private void Start()
        {
            LevelState.LoadSaveDate();
        }

        public void OnEnable()
        {
            GameEvent.PlayContinue += OnPlayContinue;
            GameEvent.PlayNewGame += OnPlayNewGame;
            GamePlayEvent.RequestLevelResource += OnRequestLevelResource;
            LevelState.OnEnable();
        }

        public void OnDisable()
        {
            LevelState.OnDisable();
            GamePlayEvent.RequestLevelResource -= OnRequestLevelResource;
        }

        private void OnPlayNewGame(int level) => LoadLevel(0);
        public void OnPlayContinue() => PlayContinueLevel();

        private void OnRequestLevelResource()
        {
            int level = LevelState.Level;
            GamePlayEvent.ResponseLevelResource?.Invoke(AllLevels[level]);
        }

        public void LoadLevel(int level)
        {
            if (level > LevelState.MaxLevel) return;

            LevelState.Level = level;
            GameEvent.LoadScene?.Invoke(AllLevels[level].sceneName);

        }

        public void PlayContinueLevel()
        {
            int level = LevelState.Level = LevelState.CompletedLevel;
            GameEvent.LoadScene?.Invoke(AllLevels[level].sceneName);
        }
    }
}