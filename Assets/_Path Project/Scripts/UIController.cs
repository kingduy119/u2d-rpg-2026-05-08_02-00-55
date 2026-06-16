using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text goldText;

    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;

    [SerializeField] private TowerData[] towers;
    private List<GameObject> activeCards = new List<GameObject>();


    void OnEnable()
    {
        Object_Spawner.OnWaveChanged += UpdateWaveText;
        TDGameManager.OnLivesChanged += UpdateLives;
        TDGameManager.OnGoldsChanged += UpdateGolds;
        Platform.OnPlatformClicked += ToggleTowerPanel;
    }

    void OnDisable()
    {
        Object_Spawner.OnWaveChanged -= UpdateWaveText;
        TDGameManager.OnLivesChanged -= UpdateLives;
        TDGameManager.OnGoldsChanged -= UpdateGolds;
        Platform.OnPlatformClicked -= ToggleTowerPanel;
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

    public void ToggleTowerPanel()
    {
        towerPanel.SetActive(!towerPanel.activeSelf);
        if (towerPanel.activeSelf)
            PopulateTowerCards();

        TDGameManager.Instance.SetTimeScale(towerPanel.activeSelf ? 0f : 1f);
    }

    private void PopulateTowerCards()
    {
        foreach (var card in activeCards)
        {
            Destroy(card);
        }
        activeCards.Clear();

        foreach (var data in towers)
        {
            GameObject card = Instantiate(towerCardPrefab, cardsContainer);
            TowerCard towerCard = card.GetComponent<TowerCard>();
            towerCard.Initialize(data);
            activeCards.Add(card);
        }
    }
}
