
namespace TDGame
{

    public class StartWaveButtonUI : ButtonBase
    {
        private int WaveNumber = 1;

        protected override void OnEnable()
        {
            base.OnEnable();
            GamePlayEvent.WaveCompleted += OnWaveCompleted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GamePlayEvent.WaveCompleted -= OnWaveCompleted;
        }

        protected override void HandleClick()
        {
            GamePlayEvent.WaveStart?.Invoke(WaveNumber);
            SetInteractable(false);
        }

        private void OnWaveCompleted()
        {
            WaveNumber++;
            SetInteractable(true);
        }


        private void SetInteractable(bool isActive)
        {
            _Button.interactable = isActive;
        }

    }
}
