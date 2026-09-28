using UnityEngine;
using UnityEngine.SceneManagement;


public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T s_Instance;
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = (T)FindAnyObjectByType(typeof(T));

                if (s_Instance == null)
                {
                    SetupInstance();
                }
                else
                {
                    string typeName = typeof(T).Name;

                    Debug.Log("[Singleton] " + typeName + " instance already created: " +
                              s_Instance.gameObject.name);
                }
            }

            return s_Instance;
        }
    }

    protected virtual void Awake() { }
    protected virtual void OnEnable()
    {
        // Clear the single instance when unloading the current scene
        SceneManager.sceneUnloaded += SceneManager_SceneUnloaded;
    }

    protected virtual void OnDisable()
    {
        if (s_Instance == this as T)
        {
            SceneManager.sceneUnloaded -= SceneManager_SceneUnloaded;
        }
    }

    private static void SetupInstance()
    {
        // lazy instantiation
        s_Instance = (T)FindAnyObjectByType(typeof(T));

        if (s_Instance == null)
        {
            GameObject gameObj = new()
            {
                name = typeof(T).Name
            };

            s_Instance = gameObj.AddComponent<T>();
            DontDestroyOnLoad(gameObj);
        }
    }
    private void SceneManager_SceneUnloaded(Scene scene)
    {
        if (s_Instance != null)
            Destroy(s_Instance.gameObject);

        s_Instance = null;
    }
}
