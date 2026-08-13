
namespace TDGame
{

    public class StartWaveButtonUI : ButtonBase
    {
        private int WaveNumber = 1;

        protected override void OnEnable()
        {
            base.OnEnable();
            // GamePlayEvent.OnActiveStartWaveButton += SetInteractable;
            GamePlayEvent.WaveCompleted += WaveCompleted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            // GamePlayEvent.OnActiveStartWaveButton -= SetInteractable;
            GamePlayEvent.WaveCompleted -= WaveCompleted;
        }

        protected override void HandleClick()
        {
            GamePlayEvent.StartWave?.Invoke(WaveNumber);
            SetInteractable(false);
        }

        private void WaveCompleted()
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
