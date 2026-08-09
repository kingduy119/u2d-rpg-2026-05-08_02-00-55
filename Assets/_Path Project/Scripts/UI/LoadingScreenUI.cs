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


        private void Awake()
        {
            StartCoroutine(LoadNextLevel("TD_Level_1"));
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadScenceAsync(sceneName));
        }

        private IEnumerator LoadScenceAsync(string sceneName)
        {
            AsyncOperation m_SceneOperation = SceneManager.LoadSceneAsync(sceneName);
            m_SceneOperation.allowSceneActivation = false;

            while (!m_SceneOperation.isDone)
            {
                float progress = Mathf.Clamp01(m_SceneOperation.progress / 0.9f);

                progressBar.value = progress;

                if (progress >= 1f)
                {
                    // Có thể chờ animation hoặc người chơi nhấn nút
                    yield return new WaitForSeconds(0.5f);

                    m_SceneOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private IEnumerator LoadNextLevel(string sceneName)
        {
            m_SceneLoadOpHandle = Addressables.LoadSceneAsync(sceneName, activateOnLoad: true);
            while (!m_SceneLoadOpHandle.IsDone)
            {
                float progress = Mathf.Clamp01(m_SceneLoadOpHandle.PercentComplete / 0.9f);

                progressBar.value = progress;

                if (progress >= 1f)
                {
                    // Có thể chờ animation hoặc người chơi nhấn nút
                    yield return new WaitForSeconds(0.5f);
                    // m_SceneLoadOpHandle.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }

}