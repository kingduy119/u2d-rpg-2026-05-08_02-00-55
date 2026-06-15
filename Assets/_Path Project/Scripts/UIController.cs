using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text goldText;

    void OnEnable()
    {
        Object_Spawner.OnWaveChanged += UpdateWaveText;
        TDGameManager.OnLivesChanged += UpdateLives;
        TDGameManager.OnGoldsChanged += UpdateGolds;
    }

    void OnDisable()
    {
        Object_Spawner.OnWaveChanged -= UpdateWaveText;
        TDGameManager.OnLivesChanged -= UpdateLives;
        TDGameManager.OnGoldsChanged += UpdateGolds;
    }

    private void UpdateWaveText(int waveIndex)
    {
        waveText.text = "Wave " + (waveIndex + 1);
    }

    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void UpdateGolds(int gold)
    {
        goldText.text = "Gold: " + gold;
    }
}
