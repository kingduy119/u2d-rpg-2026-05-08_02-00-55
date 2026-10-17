using UnityEngine;
using Characters;
using UnityEngine.UI;

public interface ICterItemSlot
{
    void Init(CharacterSO cterSO);
}
public class CterItemSlotView : ButtonView,
ICterItemSlot
{
    [SerializeField] Image image;

    [SerializeField] CharacterSO _SO;

    private void OnValidate()
    {
        UpdateView();
    }

    public void Init(CharacterSO cterSO)
    {
        _SO = cterSO;
        UpdateView();
    }

    private void UpdateView()
    {
        if (_SO && _SO.colors.Count > 0)
        {
            image.sprite = _SO.colors[0].Sprite;
        }
    }

    protected override void HandleClick()
    {
        Debug.Log("CterItemSlotView Click");
        // Selected();
        // ButtonEvents.CterDetailSlotView?.Invoke(_SO);
    }
}
