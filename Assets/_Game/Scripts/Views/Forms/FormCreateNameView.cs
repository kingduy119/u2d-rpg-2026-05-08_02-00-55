using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FormCreateNameView : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button submitButton;

    // public event Action<string> OnSubmit;

    private void Awake()
    {
        submitButton.onClick.AddListener(OnSubmitClicked);
    }

    private void OnSubmitClicked()
    {
        string nickname = inputField.text.Trim();
        if (!string.IsNullOrEmpty(nickname))
        {
            // OnSubmit?.Invoke(name);
            ButtonEvents.ConfirmChangeNickname?.Invoke(nickname);
        }
        else
        {
            Debug.LogWarning("Name cannot be empty.");
        }
    }
}