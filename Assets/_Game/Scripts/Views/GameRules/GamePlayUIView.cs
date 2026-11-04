

using System.Collections.Generic;
using UnityEngine;

public class GamePlayUIView : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _confirmButton;

    [SerializeField] GameObject _cardPrefab;
    [SerializeField] GameObject _listCard;
    public List<CharacterCardSO> SelectCards;

    public ListView PlayerHealth;
    public ListView EnemyHealth;

    void Start()
    {
        RefreshListCard();
    }

    private void RefreshListCard()
    {
        if (_listCard != null && _cardPrefab != null)
        {
            foreach (Transform child in _listCard.transform)
            {
                var item = child.Find("GamePlayCardSelect");

                if (item == null) continue;

                if (!item.TryGetComponent<GamePlayCardSelect>(out var card))
                    continue;

                var so = SelectCards[Random.Range(0, SelectCards.Count)];

                card.Init(so.CharacterSO, so.Resources);
                card.Visible();
            }
        }
    }

    public void IdleExit()
    {
        _listCard.SetActive(false);
        _startButton.SetActive(false);
    }

    public void IdleEnter()
    {
        _listCard.SetActive(true);
        _startButton.SetActive(true);
        _confirmButton.SetActive(false);
    }

    public void ShowConfirm(string message = "")
    {
        Debug.Log(message);
        _confirmButton.SetActive(true);
    }
    public void HideConfirm() => _confirmButton.SetActive(false);
}