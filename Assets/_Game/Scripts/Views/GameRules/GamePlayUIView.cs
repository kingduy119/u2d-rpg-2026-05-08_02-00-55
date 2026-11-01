

using System.Collections.Generic;
using UnityEngine;

public class GamePlayUIView : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _confirmButton;

    [SerializeField] GameObject _cardPrefab;
    [SerializeField] GameObject _listCard;
    public List<CharacterCardSO> SelectCards;

    void Start()
    {
        RefreshListCard();
    }

    private void RefreshListCard()
    {
        if (_listCard != null && _cardPrefab != null)
        {
            foreach (Transform item in _listCard.transform) Destroy(item.gameObject);
            foreach (var cardSO in SelectCards)
            {
                var item = Instantiate(_cardPrefab, _listCard.transform);
                if (item.TryGetComponent<GamePlayCardSelect>(out var card))
                {
                    card.Init(cardSO.CharacterSO, cardSO.Resources);
                }
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

    public void ToggleConfirm(string message = "")
    {
        Debug.Log(message);
        _confirmButton.SetActive(!_confirmButton.activeSelf);
    }
}