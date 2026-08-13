using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{
    public class Setting
    {
        string _KeyAsset;
        AsyncOperationHandle<GameObject> loader;
        GameObject SettingUI;

        public Setting(string key)
        {
            _KeyAsset = key;
            Coroutines.StartCoroutine(LoadAsset());
        }

        IEnumerator LoadAsset()
        {
            loader = Addressables.LoadAssetAsync<GameObject>(_KeyAsset);
            yield return loader;
        }

        public GameObject GetUI(Transform transform = null)
        {
            if (loader.Status == AsyncOperationStatus.Succeeded && SettingUI == null)
            {
                var op = Addressables.InstantiateAsync(_KeyAsset, transform);
                SettingUI = op.Result;
            }

            return SettingUI;
        }

    }

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

        // [SerializeField] private SettingUI _SettingUIPrefab;
        // private SettingUI _SettingUI;
        // public SettingUI SettingUI => Lazy.Load(ref _SettingUI, _SettingUIPrefab, gameObject.transform);


        // private void Awake()
        // {
        //     // SettingUI.gameObject.SetActive(false);
        // }
        // private GameObject _SettingUI;
        // private AsyncOperationHandle<GameObject> _handle;

        // private void Start()
        // {
        //     // StartCoroutine(LoadAssets());
        // }

        // IEnumerator LoadAssets()
        // {
        //     string key = "Game/SettingUI";
        //     AsyncOperationHandle<GameObject> loadOp = Addressables.LoadAssetAsync<GameObject>(key);
        //     yield return loadOp;

        //     if (loadOp.Status == AsyncOperationStatus.Succeeded)
        //     {
        //         var op = Addressables.InstantiateAsync(key);
        //         _SettingUI = op.Result;
        //         if (op.IsDone)
        //         {
        //             //...
        //         }
        //         //...
        //     }
        //     // _handle = Addressables.InstantiateAsync("Game/SettingUI", transform);
        //     // yield return _handle;

        //     // if (_handle.Status == AsyncOperationStatus.Succeeded)
        //     // {
        //     //     _SettingUI = _handle.Result;
        //     // }
        // }

        Setting _Setting;
        GameObject SettingUI => _Setting.GetUI(transform);


        private void Awake()
        {
            _Setting = new("Game/SettingsUI");
        }

        private void OnEnable()
        {
            GamePlayEvent.ResponseUpdateUI += OnUpdateGamePlayUI;

            GamePlayEvent.SettingClick += OnSettingClick;
            GamePlayEvent.SettingClose += OnSettingClose;


            GamePlayEvent.RequestUpdateUI?.Invoke();
        }

        private void OnDisable()
        {
            GamePlayEvent.ResponseUpdateUI -= OnUpdateGamePlayUI;

            GamePlayEvent.SettingClick -= OnSettingClick;
            GamePlayEvent.SettingClose -= OnSettingClose;
        }

        private void OnDestroy()
        {
            // Addressables.Release(_handle);
        }

        private void OnSettingClick()
        {
            SettingUI.SetActive(true);
        }

        private void OnSettingClose()
        {
            SettingUI.SetActive(false);
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


        private void OpenSettingUI()
        {
            // SettingUI.gameObject.SetActive(true);
            // GameEvent.PauseGame?.Invoke();
        }
        private void CloseSettingUI()
        {
            // SettingUI.gameObject.SetActive(false);
            // GameEvent.ResumeGame?.Invoke();
        }
    }
}