using TMPro;
using UnityEngine;
using VContainer;



public class UserProfileView : MonoBehaviour
{
    [SerializeField] TMP_Text _nickName;
    // [Inject] GameManager _GameManager;

    public void UpdateView(UserData user)
    {
        if (user == null) return;
        SetNickName(user.Nickname);
    }

    public void SetNickName(string nickname)
    {
        if (_nickName != null) _nickName.text = nickname;
    }
}
