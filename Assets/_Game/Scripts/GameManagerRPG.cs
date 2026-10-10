using UnityEngine;

public class GameManagerRPG : MonoBehaviour
{
    public static GameManagerRPG Instance;

    public GameObject[] persistentObjects;

    public DialogueManager dialogueManager;
    public DialogueHistoryTracker dialogueHistoryTracker;
    public LocationHistoryTracker locationHistoryTracker;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            MarkPersistentObjects();
        }
        else
        {
            CleanUpAndDestroy();
        }
    }


    private void MarkPersistentObjects()
    {
        foreach (GameObject obj in persistentObjects)
        {
            DontDestroyOnLoad(obj);
        }
    }

    private void CleanUpAndDestroy()
    {
        foreach (GameObject obj in persistentObjects)
        {
            Destroy(obj);
        }
        Destroy(gameObject);
    }
}
