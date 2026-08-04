
namespace TDGame
{

    public class StartWaveButtonUI : ButtonBase
    {
        private int WaveNumber = 1;

        protected override void OnEnable()
        {
            base.OnEnable();
            // InGameEvent.OnActiveStartWaveButton += SetInteractable;
            InGameEvent.WaveCompleted += WaveCompleted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            // InGameEvent.OnActiveStartWaveButton -= SetInteractable;
            InGameEvent.WaveCompleted -= WaveCompleted;
        }

        protected override void HandleClick()
        {
            InGameEvent.StartWave?.Invoke(WaveNumber);
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
