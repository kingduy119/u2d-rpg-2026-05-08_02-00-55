using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
            // Destroy(gameObject);
            CleanUpAndDestroy();
        }
    }

    void Start()
    {

    }

    void Update()
    {

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
