
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _text;
    [SerializeField] ResourceItemSO _so;
    [SerializeField] ItemDTO _dto;

    private int _value = 0;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            SetText($"{_value}");
        }
    }

    void OnValidate()
    {
        if (_dto) UpdateView(_dto);

        if (_so == null) return;
        _icon.sprite = _so.Icon;
        _text.text = $"{_so.Value}";

    }

    void Start()
    {
        // if (_so) _value = _so.Value;
        if (_dto) UpdateView(_dto);
    }

    public void VisibleIfZero(int value)
    {
        Value = value;
        gameObject.SetActive(value > 0);
    }

    public void UpdateView(ItemDTO dto)
    {
        SetIcon(dto.Icon);
        SetText($"{Value}");
    }

    private void SetIcon(Sprite sprite) { if (_icon) _icon.sprite = sprite; }
    private void SetText(string text) { if (_text) _text.text = text; }
}