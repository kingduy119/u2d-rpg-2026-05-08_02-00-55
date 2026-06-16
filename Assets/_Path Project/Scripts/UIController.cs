using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text alertText;

    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;

    [SerializeField] private TowerData[] towers;
    private List<GameObject> activeCards = new List<GameObject>();

    private Platform _currentPlatform;

    void OnEnable()
    {
        Object_Spawner.OnWaveChanged += UpdateWaveText;
        TDGameManager.OnLivesChanged += UpdateLives;
        TDGameManager.OnGoldsChanged += UpdateGolds;
        Platform.OnPlatformClicked += OpenTowerPanel;
        TowerCard.OnTowerCardSelected += HandleTowerCardSelected;
    }

    void OnDisable()
    {
        Object_Spawner.OnWaveChanged -= UpdateWaveText;
        TDGameManager.OnLivesChanged -= UpdateLives;
        TDGameManager.OnGoldsChanged -= UpdateGolds;
        Platform.OnPlatformClicked -= OpenTowerPanel;
        TowerCard.OnTowerCardSelected -= HandleTowerCardSelected;
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

    public void OpenTowerPanel(Platform platform)
    {
        _currentPlatform = platform;
        towerPanel.SetActive(true);
        PopulateTowerCards();
        TDGameManager.Instance.SetTimeScale(0f);
    }

    public void ShowAlert(string message)
    {
        alertText.text = message;
        alertText.gameObject.SetActive(true);
        // Invoke(nameof(HideAlert), 2f);
    }

    private void HideAlert()
    {
        alertText.gameObject.SetActive(false);
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

    public void HandleTowerCardSelected(TowerData data)
    {
        if (TDGameManager.Instance.Golds < data.cost)
        {
            StartCoroutine(ShowAlertCoroutine("Not enough gold!"));
            return;
        }
        if (_currentPlatform != null)
        {
            TDGameManager.Instance.SpendGold(data.cost);
            _currentPlatform.PlaceTower(data);
            ToggleTowerPanel();
        }
    }

    private System.Collections.IEnumerator ShowAlertCoroutine(string message)
    {
        ShowAlert(message);
        yield return new WaitForSeconds(2f);
        HideAlert();
    }
}
