using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;



public class UserProfileView : MonoBehaviour
{
    [SerializeField] TMP_Text _nickName;
    [SerializeField] ItemView _Diamond;
    [SerializeField] ItemView _Gold;

    [Inject] ProfileSerice _ProfileSerice;

    void OnEnable()
    {
        if (_ProfileSerice == null) return;
        _ProfileSerice.OnResourceChange += UpdateView;

        UpdateView(_ProfileSerice.UserResource);
    }
    void OnDisable()
    {
        if (_ProfileSerice == null) return;
        _ProfileSerice.OnResourceChange -= UpdateView;
    }

    public void UpdateView(UserData user)
    {
        if (user == null) return;
        SetNickName(user.Nickname);
    }

    public void UpdateView(ResourceData resource)
    {
        if (_Diamond) _Diamond.Value = resource.Diamond;
        if (_Gold) _Gold.Value = resource.Gold;
    }

    public void SetNickName(string nickname)
    {
        if (_nickName != null) _nickName.text = nickname;
    }
}
