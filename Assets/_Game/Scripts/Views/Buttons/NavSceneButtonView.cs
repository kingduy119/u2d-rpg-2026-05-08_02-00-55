using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;



public class NavSceneButtonView : ButtonBase
{
    [SerializeField] private string _sceneName;

    // private DownloadService _DownloadService;

    // [Inject]
    // public void Construct(DownloadService service)
    // {
    //     _DownloadService = service;
    //     Debug.Log($"NavSceneButtonView.Construct: {_sceneName} - {_DownloadService != null}");
    // }

    protected override void HandleClick()
    {
        // Debug.Log($"NavSceneButtonView.Construct: {_sceneName} - {_DownloadService != null}");
        // _DownloadService?.LoadScene(_sceneName).Forget();
        SceneEvents.ChangeScene?.Invoke(_sceneName);
    }
}




[RequireComponent(typeof(Button))]
public abstract class ButtonBase : MonoBehaviour
{
    protected Button _Button;

    protected virtual void Awake()
    {
        _Button = GetComponent<Button>();

    }

    protected virtual void OnEnable()
    {
        _Button.onClick.AddListener(HandleClick);
    }

    protected virtual void OnDisable()
    {
        _Button.onClick.RemoveListener(HandleClick);
    }


    protected abstract void HandleClick();
}