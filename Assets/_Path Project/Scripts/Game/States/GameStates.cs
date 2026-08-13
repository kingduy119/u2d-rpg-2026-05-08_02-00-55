
namespace TDGame
{
    public class GameStates : StateMachine
    {
        public GameMenuState GameMenuState { get; private set; }
        public GamePlayState GamePlayState { get; private set; }

        public GameStates(GameManager gm)
        {
            GameMenuState = new GameMenuState(gm);
            GamePlayState = new GamePlayState(gm);

            GameMenuState.Enter();
            CurrentState = GameMenuState;
        }
    }
}