

using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayUIView : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _confirmPopup;
    [SerializeField] TMP_Text _confirmMessage;
    [SerializeField] TMP_Text _goldResetText;

    [SerializeField] ListView _listCardView;
    public List<CharacterCardSO> SelectCards;

    public ListView PlayerHealth;


    public void RefreshListCard()
    {
        if (_listCardView != null)
        {
            _listCardView.LoopItemInChild<GamePlayCardSelect>("GamePlayCardSelect", item =>
            {
                var so = SelectCards[Random.Range(0, SelectCards.Count)];
                item.Init(so.CharacterSO, so.Resources);
                item.Visible();
            });
        }
    }

    public void IdleExit()
    {
        _listCardView.gameObject.SetActive(false);
        _startButton.SetActive(false);
    }

    public void IdleEnter()
    {
        _listCardView.gameObject.SetActive(true);
        _startButton.SetActive(true);
        _confirmPopup.SetActive(false);
    }

    public void ShowConfirm(string message = "")
    {
        _confirmMessage.text = message;
        _confirmPopup.SetActive(true);
    }
    public void HideConfirm() => _confirmPopup.SetActive(false);
}