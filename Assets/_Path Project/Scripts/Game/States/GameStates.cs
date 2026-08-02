
namespace TDGame
{
    public class GameStates : StateMachine
    {
        public GameMenuState GameMenuState { get; private set; }
        public GamePlayState GamePlayState { get; private set; }

        public GameStates()
        {
            GameMenuState = new GameMenuState();
            GamePlayState = new GamePlayState();

            CurrentState = GameMenuState;
        }
    }
}