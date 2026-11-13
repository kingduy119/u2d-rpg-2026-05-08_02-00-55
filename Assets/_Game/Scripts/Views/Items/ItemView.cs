
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _text;
    [SerializeField] ResourceItemSO _so;

    private int _value = 0;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            _text.text = $"{value}";
        }
    }

    void OnValidate()
    {
        if (_so == null) return;
        _icon.sprite = _so.Icon;
        _text.text = $"{_so.Value}";
    }

    void Start()
    {
        if (_so) _value = _so.Value;
    }

    public void VisibleIfZero(int value)
    {
        Value = value;
        gameObject.SetActive(value > 0);
    }
}