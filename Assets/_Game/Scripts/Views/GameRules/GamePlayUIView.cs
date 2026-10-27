

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
        _startButton.SetActive(false);
        _listCard.SetActive(false);
    }

    public void IdleEnter()
    {
        _startButton.SetActive(true);
        _listCard.SetActive(true);
    }

    public void ToggleConfirm(string message = "")
    {
        Debug.Log(message);
        _confirmButton.SetActive(!_confirmButton.activeSelf);
    }
}