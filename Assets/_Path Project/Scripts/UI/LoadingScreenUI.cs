using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Unity.VisualScripting;



namespace TDGame
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        AsyncOperationHandle<SceneInstance> handle;

        private IEnumerator Start()
        {
            yield return LoadGameScene(GameManager.SceneName);
        }

        private IEnumerator LoadGameScene(string sceneName)
        {
            handle = Addressables.LoadSceneAsync(
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
                // yield return new WaitForSeconds(0.5f);
            }

            // Kiểm tra load có thành công không
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                yield break;
            }

            // Loading bar đã đạt 100%
            progressBar.value = 1f;
            // Có thể chờ một chút để người chơi thấy loading bar 100%
            yield return new WaitForSeconds(1f);

            GameEvent.LoadingSceneDone?.Invoke();

            yield return handle.Result.ActivateAsync();
        }

    }

}