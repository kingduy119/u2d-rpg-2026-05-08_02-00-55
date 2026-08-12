
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{
    public class GameMenuState : IState
    {
        private GameObject _MainMenuUI;
        private AsyncOperationHandle<GameObject> handle;
        private GameManager _GameManager;

        public GameMenuState(GameManager gm)
        {
            _GameManager = gm;
            Coroutines.StartCoroutine(LoadAsset());
        }

        public void Enter()
        {
            GameEvent.PlayNewGame += OnPlayNewGame;
            GameEvent.PlayContinue += OnPlayContinue;
            _MainMenuUI.SetActive(true);
        }

        public void Exit()
        {
            GameEvent.PlayNewGame -= OnPlayNewGame;
            GameEvent.PlayContinue -= OnPlayContinue;
            _MainMenuUI.SetActive(false);
        }

        IEnumerator LoadAsset()
        {
            handle = Addressables.InstantiateAsync("Game/MainMenu");
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _MainMenuUI = handle.Result;
            }
        }

        private void OnPlayNewGame(int level)
        {
            // UIManager.Instance.InGameUI.gameObject.SetActive(true);
            // GameStates.TransitionTo(GameStates.GamePlayState);
            // LevelManager.LoadLevel(level);
            _GameManager.GameStates.TransitionTo(_GameManager.GameStates.GamePlayState);
        }

        public void OnPlayContinue()
        {
            // UIManager.Instance.InGameUI.gameObject.SetActive(true);
            // GameStates.TransitionTo(GameStates.GamePlayState);
            // LevelManager.PlayContinueLevel();
            _GameManager.GameStates.TransitionTo(_GameManager.GameStates.GamePlayState);
        }

    }

}