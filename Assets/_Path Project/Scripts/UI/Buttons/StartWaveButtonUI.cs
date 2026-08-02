
namespace TDGame
{

    public class StartWaveButtonUI : ButtonBase
    {

        protected override void OnEnable()
        {
            base.OnEnable();
            InGameEvent.OnActiveStartWaveButton += SetInteractable;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            InGameEvent.OnActiveStartWaveButton -= SetInteractable;
        }

        protected override void HandleClick()
        {
            InGameEvent.OnStartWave?.Invoke();
        }


        private void SetInteractable(bool isActive)
        {
            _Button.interactable = isActive;
        }

    }
}
