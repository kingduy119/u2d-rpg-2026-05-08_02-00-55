using TMPro;
using UnityEngine;

namespace TDGame
{

    public class GamePlayUI : MonoBehaviour
    {
        [Header("UI Text")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text rockText;
        [SerializeField] private TMP_Text woodText;
        [SerializeField] private TMP_Text alertText;
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Text enemiesText;

        AssetLoader _SettingLoader,
                    _MissionCompleteLoader,
                    _TowerBuildLoader,
                    _TowerSelectLoader;
        GameObject SettingUI => _SettingLoader.Instantiate(transform);
        GameObject MissionCompleteUI => _MissionCompleteLoader.Instantiate(transform);
        GameObject TowerBuildCursor => _TowerBuildLoader.Instantiate();
        GameObject TowerSelectCursor => _TowerSelectLoader.Instantiate();


        private void Awake()
        {
            _SettingLoader = new("Game/SettingsUI", true);
            _MissionCompleteLoader = new("Game/MissionCompletedUI", true);
            _TowerBuildLoader = new("Tower/TowerBuildCursor", true);
            _TowerSelectLoader = new("Tower/TowerSelectCursor", true);

        }

        private void OnEnable()
        {

            GamePlayEvent.SettingClick += OnSettingClick;
            GamePlayEvent.SettingClose += OnSettingClose;
            GamePlayEvent.MissionComplete += OnMissionComplete;

            GamePlayEvent.ShowSelectCursor += OnShowSelectCursor;
            GamePlayEvent.HideSelectCursor += OnHideSelectCursor;
            GamePlayEvent.ShowBuildCursor += OnShowBuildCursor;
            GamePlayEvent.HideBuildCursor += OnHideBuildCursor;

            GamePlayEvent.ResponseUpdateUI += OnUpdateGamePlayUI;
            GamePlayEvent.RequestUpdateUI?.Invoke();
        }

        private void OnDisable()
        {
            GamePlayEvent.SettingClick -= OnSettingClick;
            GamePlayEvent.SettingClose -= OnSettingClose;
            GamePlayEvent.MissionComplete -= OnMissionComplete;

            GamePlayEvent.ShowSelectCursor -= OnShowSelectCursor;
            GamePlayEvent.HideSelectCursor -= OnHideSelectCursor;
            GamePlayEvent.ShowBuildCursor -= OnShowBuildCursor;
            GamePlayEvent.HideBuildCursor -= OnHideBuildCursor;

            GamePlayEvent.ResponseUpdateUI -= OnUpdateGamePlayUI;
        }

        private void OnDestroy()
        {
            _SettingLoader.Release();
            _MissionCompleteLoader.Release();
            _TowerBuildLoader.Release();
            _TowerSelectLoader.Release();
        }
        private void OnUpdateGamePlayUI(GamePlayState state)
        {
            goldText.SetText("{0}", state.Golds);
            rockText.SetText("{0}", state.Rocks);
            woodText.SetText("{0}", state.Woods);
            livesText.SetText("{0}", state.Lives);
            waveText.SetText("{0}", state.WaveCount + 1);
            enemiesText.SetText("{0}", state.Enemies);
        }

        private void OnSettingClick() => SettingUI.SetActive(true);
        private void OnSettingClose() => SettingUI.SetActive(false);
        private void OnMissionComplete() => MissionCompleteUI.SetActive(true);

        private void OnHideSelectCursor() => TowerSelectCursor.SetActive(false);
        private void OnShowSelectCursor(Vector3 position)
        {
            TowerSelectCursor.transform.position = position;
            TowerSelectCursor.SetActive(true);
        }
        private void OnHideBuildCursor() => TowerBuildCursor.SetActive(false);
        private void OnShowBuildCursor(Vector3 position)
        {
            TowerBuildCursor.transform.position = position;
            TowerBuildCursor.SetActive(true);
        }
    }
}