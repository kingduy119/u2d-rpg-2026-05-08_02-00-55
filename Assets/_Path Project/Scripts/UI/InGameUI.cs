using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public static class Lazy
    {
        public static T Load<T>(ref T instance, T prefab, Transform parent = null)
            where T : Object
        {
            if (instance == null)
            {
                instance = Object.Instantiate(prefab, parent);
            }

            return instance;
        }
    }

    public class InGameUI : MonoBehaviour
    {
        [Header("UI Text")]
        [SerializeField] private TMP_Text goldText;
        [SerializeField] private TMP_Text rockText;
        [SerializeField] private TMP_Text woodText;
        [SerializeField] private TMP_Text alertText;
        [SerializeField] private TMP_Text waveText;
        [SerializeField] private TMP_Text livesText;
        [SerializeField] private TMP_Text enemiesText;


        [SerializeField] private SettingUI _SettingUIPrefab;
        private SettingUI _SettingUI;
        public SettingUI SettingUI => Lazy.Load(ref _SettingUI, _SettingUIPrefab, gameObject.transform);


        private void Awake()
        {
            SettingUI.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            InGameEvent.OnUpdateUI += UpdateInGameUI;
            GameEvent.SettingOpen += OpenSettingUI;
            GameEvent.SettingClose += CloseSettingUI;
        }

        private void OnDisable()
        {
            InGameEvent.OnUpdateUI -= UpdateInGameUI;
            GameEvent.SettingOpen -= OpenSettingUI;
            GameEvent.SettingClose -= CloseSettingUI;
        }

        private void UpdateInGameUI(GamePlayState state)
        {
            goldText.SetText("{0}", state.Golds);
            rockText.SetText("{0}", state.Rocks);
            woodText.SetText("{0}", state.Woods);
            livesText.SetText("{0}", state.Lives);
            waveText.SetText("{0}", state.WaveCount + 1);
            enemiesText.SetText("{0}", state.Enemies);
        }

        // public void OnCloseSettingsClick() => ResumeGame();
        private void OpenSettingUI()
        {
            Debug.Log("OPenSetingg");
            SettingUI.gameObject.SetActive(true);
            GameEvent.PauseGame?.Invoke();
        }
        private void CloseSettingUI()
        {
            SettingUI.gameObject.SetActive(false);
            GameEvent.ResumeGame?.Invoke();
        }
    }
}