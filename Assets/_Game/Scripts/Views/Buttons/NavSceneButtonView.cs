using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;



public class NavSceneButtonView : ButtonBase
{
    [SerializeField] private string _sceneName;

    private DownloadService _service;

    [Inject]
    public void Construct(DownloadService service)
    {
        _service = service;
        Debug.Log($"NavSceneButtonView.Construct: {_sceneName} - {_service != null}");
    }

    protected override void HandleClick()
    {
        Debug.Log($"NavSceneButtonView.HandleClick: {_sceneName}");
        _service?.LoadSceneByName(_sceneName).Forget();
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