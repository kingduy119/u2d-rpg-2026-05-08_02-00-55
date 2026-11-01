
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _text;
    [SerializeField] ResourceItemSO _so;

    void OnValidate()
    {
        if (_so == null) return;
        _icon.sprite = _so.Icon;
        _text.text = $"{_so.Value}";
    }

    public int Value
    {
        get => _so.Value;
        set
        {
            _so.Value = value;
            _text.text = $"{_so.Value}";
        }
    }
}