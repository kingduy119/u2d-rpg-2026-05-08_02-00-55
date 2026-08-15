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

        AssetLoader _SettingLoader, _MissionCompleteLoader;
        GameObject SettingUI => _SettingLoader.Instantiate(transform);
        GameObject MissionCompleteUI => _MissionCompleteLoader.Instantiate(transform);


        private void Awake()
        {
            _SettingLoader = new("Game/SettingsUI", true);
            _MissionCompleteLoader = new("Game/MissionCompletedUI", true);
        }

        private void OnEnable()
        {
            GamePlayEvent.ResponseUpdateUI += OnUpdateGamePlayUI;

            GamePlayEvent.SettingClick += OnSettingClick;
            GamePlayEvent.SettingClose += OnSettingClose;
            GamePlayEvent.MissionComplete += OnMissionComplete;
            // GamePlayEvent.MissionCompleteClick += OnMissionCompleteClick;


            GamePlayEvent.RequestUpdateUI?.Invoke();
        }

        private void OnDisable()
        {
            GamePlayEvent.ResponseUpdateUI -= OnUpdateGamePlayUI;

            GamePlayEvent.SettingClick -= OnSettingClick;
            GamePlayEvent.SettingClose -= OnSettingClose;
            GamePlayEvent.MissionComplete -= OnMissionComplete;
            // GamePlayEvent.MissionCompleteClick -= OnMissionCompleteClick;
        }

        private void OnDestroy()
        {
            _SettingLoader.Release();
            _MissionCompleteLoader.Release();
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
        // private void OnMissionCompleteClick() => GameEvent.LoadScene("TD_MainMenu");

    }
}