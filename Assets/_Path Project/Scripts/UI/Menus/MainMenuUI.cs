using TMPro;
using UnityEngine;

namespace TDGame
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text txtLevel;

        // private void OnEnable()
        // {
        //     GameEvent.OnUpdateUI += UpdateUI;
        // }

        // private void OnDisable()
        // {
        //     GameEvent.OnUpdateUI -= UpdateUI;
        // }

        // private void UpdateUI()
        // {
        //     txtLevel.text = GameManager.Instance.LevelManager.LevelState.CurrentLevel.levelName;
        // }
    }
}