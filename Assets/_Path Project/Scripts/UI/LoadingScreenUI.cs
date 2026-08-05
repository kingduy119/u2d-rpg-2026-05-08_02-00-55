using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


namespace TDGame
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider progressBar;

        void Start()
        {
            progressBar.value = 0;
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadScenceAsync(sceneName));
        }

        private IEnumerator LoadScenceAsync(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                progressBar.value = progress;

                if (progress >= 1f)
                {
                    // Có thể chờ animation hoặc người chơi nhấn nút
                    yield return new WaitForSeconds(0.5f);

                    operation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }

}