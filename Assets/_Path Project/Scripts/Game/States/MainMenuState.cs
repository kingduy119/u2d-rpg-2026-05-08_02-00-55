
using UnityEngine.SceneManagement;

namespace TDGame
{
    public class GameSetupState :
        DirtyState,
        IState
    {
        private GameManager GM;
        public GameSetupState(GameManager gm)
        {
            GM = gm;
        }

        public void Enter()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void Exit()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GM.StateMachine.TransitionTo(GM.MenuState);
            GameEvent.LoadScene?.Invoke("TD_MainMenu");
        }
    }

    public class GameMenuState : IState
    {
        readonly GameManager GM;
        public GameMenuState(GameManager gm)
        {
            GM = gm;
        }

        public void Enter()
        {
            GameEvent.PlayContinue += OnPlayContinue;
            GameEvent.PlayNewGame += OnPlayNewGame;

        }

        public void Exit()
        {
            GameEvent.PlayContinue -= OnPlayContinue;
            GameEvent.PlayNewGame -= OnPlayNewGame;

        }

        private void OnPlayNewGame(int level)
        {
            GM.StateMachine.TransitionTo(GM.GamePlayState);

        }

        public void OnPlayContinue()
        {
            GM.StateMachine.TransitionTo(GM.GamePlayState);

        }
    }

}