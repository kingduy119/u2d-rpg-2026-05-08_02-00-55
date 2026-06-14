using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;

    void OnEnable()
    {
        Object_Spawner.OnWaveChanged += UpdateWaveText;
        TDGameManager.OnLivesChanged += UpdateLives;
    }

    void OnDisable()
    {
        Object_Spawner.OnWaveChanged -= UpdateWaveText;
        TDGameManager.OnLivesChanged -= UpdateLives;
    }

    private void UpdateWaveText(int waveIndex)
    {
        waveText.text = "Wave " + (waveIndex + 1);
    }

    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }
}
