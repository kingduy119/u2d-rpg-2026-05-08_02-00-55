using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class CameraConfinerFinder : MonoBehaviour
{
    // void Start()
    // {
    //     CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();
    //     confiner.BoundingShape2D = GameObject.FindWithTag("Confiner").GetComponent<PolygonCollider2D>();
    // }

    void OnEnable()
    {
        // SceneChanger.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();
        confiner.BoundingShape2D = GameObject.FindWithTag("Confiner").GetComponent<PolygonCollider2D>();
    }

}
