
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] TMP_Text _text;
    [SerializeField] bool _enableValidate = true;

    [SerializeField] ItemDto _dto;
    public ItemDto DTO
    {
        get => _dto;
        set
        {
            _dto = value;
            UpdateView(_dto);
        }
    }

    [SerializeField] float _value = 0;
    public float Value
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
        if (_enableValidate && _dto) UpdateView(_dto);
    }

    void Start()
    {
        if (_dto) UpdateView(_dto);
    }

    public void VisibleIfZero(int value)
    {
        Value = value;
        gameObject.SetActive(value > 0);
    }

    public void UpdateView(ItemDto dto)
    {
        SetIcon(dto.Icon);
        // SetText($"{Value}");
    }

    public void SetActive(bool active) => gameObject.SetActive(active);
    public void SetText(string text) { if (_text) _text.text = text; }
    public void SetIcon(Sprite sprite) { if (_icon) _icon.sprite = sprite; }
}