using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;



public class UserProfileView : MonoBehaviour
{
    [SerializeField] TMP_Text _nickName;
    [SerializeField] ItemView _Diamond;
    [SerializeField] ItemView _Gold;

    [Inject] readonly ProfileSerice _ProfileSerice;

    void Start()
    {
        if (_ProfileSerice != null)
        {
            UpdateView(_ProfileSerice);
        }
    }

    void OnEnable()
    {
        if (_ProfileSerice == null) return;
        _ProfileSerice.OnChange += UpdateView;

        UpdateView(_ProfileSerice.UserResource);
    }
    void OnDisable()
    {
        if (_ProfileSerice == null) return;
        _ProfileSerice.OnChange -= UpdateView;
    }

    private void UpdateView(ProfileSerice profile)
    {
        UpdateView(profile.User);
        UpdateView(profile.UserResource);
    }

    public void UpdateView(UserData user)
    {
        if (user == null) return;
        SetNickName(user.Nickname);
    }

    public void UpdateView(ResourceData resource)
    {
        if (resource == null) return;
        if (_Diamond) _Diamond.Value = resource.Diamond;
        if (_Gold) _Gold.Value = resource.Gold;
    }

    public void SetNickName(string nickname)
    {
        if (_nickName != null) _nickName.text = nickname;
    }
}
