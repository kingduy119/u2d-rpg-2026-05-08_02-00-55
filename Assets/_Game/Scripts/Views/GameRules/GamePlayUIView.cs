

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayUIView : MonoBehaviour
{
    [SerializeField] ButtonView _startButton;
    [SerializeField] GameObject _listCard;

    // public 
    public void Hide()
    {
        _startButton.gameObject.SetActive(false);
        _listCard.gameObject.SetActive(false);
    }

    public void Show()
    {
        _startButton.gameObject.SetActive(true);
        _listCard.gameObject.SetActive(true);
    }
}