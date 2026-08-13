using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;



namespace TDGame
{
    public class LoadingScreen : MonoBehaviour
    {
        private static AsyncOperationHandle<SceneInstance> m_SceneLoadOpHandle;

        [SerializeField] private Slider progressBar;

        private IEnumerator Start()
        {
            Debug.Log("LoadingStart");
            yield return LoadGameScene(GameManager.SceneName);
        }

        private IEnumerator LoadGameScene(string sceneName)
        {
            AsyncOperationHandle<SceneInstance> handle =
                Addressables.LoadSceneAsync(
                    sceneName,
                    LoadSceneMode.Single,
                    activateOnLoad: false
                );

            // Loading
            while (!handle.IsDone)
            {
                float progress = handle.PercentComplete;

                progressBar.value = progress;

                yield return null;
            }

            // Kiểm tra load có thành công không
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load {sceneName}");
                yield break;
            }

            Debug.Log($"{sceneName} loaded!");

            // Loading bar đã đạt 100%
            progressBar.value = 1f;

            // Có thể chờ một chút để người chơi thấy loading bar 100%
            yield return new WaitForSeconds(0.5f);

            GameEvent.LoadingDone?.Invoke();

            // Activate scene
            yield return handle.Result.ActivateAsync();

        }
    }

}