
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
        private readonly GameManager GM;

        public GameMenuState(GameManager gm)
        {
            GM = gm;
            Coroutines.StartCoroutine(LoadAsset());
        }

        public void Enter()
        {
            GameEvent.PlayContinue += OnPlayContinue;
            GameEvent.PlayNewGame += OnPlayNewGame;

            if (_MainMenuUI != null) _MainMenuUI.SetActive(true);
        }

        public void Exit()
        {
            GameEvent.PlayContinue -= OnPlayContinue;
            GameEvent.PlayNewGame -= OnPlayNewGame;

            if (_MainMenuUI != null) _MainMenuUI.SetActive(false);
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
            GM.GameStates.TransitionTo(GM.GameStates.GamePlayState);
            GM.LevelManager.LoadLevel(0);
        }

        public void OnPlayContinue()
        {
            GM.GameStates.TransitionTo(GM.GameStates.GamePlayState);
            GM.LevelManager.PlayContinueLevel();
        }
    }

}