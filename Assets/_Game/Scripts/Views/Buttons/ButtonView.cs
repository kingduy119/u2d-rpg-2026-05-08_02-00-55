using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonView : MonoBehaviour
{
    protected Button _Button;

    protected virtual void Awake()
    {
        _Button = GetComponent<Button>();

    }

    protected virtual void Start() { }

    protected virtual void OnEnable()
    {
        _Button.onClick.AddListener(HandleClick);
    }

    protected virtual void OnDisable()
    {
        _Button.onClick.RemoveListener(HandleClick);
    }

    protected abstract void HandleClick();

    public virtual void Selected() => _Button.interactable = false;
    public virtual void UnSelected() => _Button.interactable = true;
    public virtual void Hidden() => gameObject.SetActive(false);
    public virtual void Visible() => gameObject.SetActive(true);
}