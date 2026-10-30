

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlayUIView : MonoBehaviour
{
    [SerializeField] GameObject _startButton;
    [SerializeField] GameObject _confirmButton;
    [SerializeField] GameObject _listCard;

    // public 
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