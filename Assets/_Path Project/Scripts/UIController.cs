using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text alertText;
    [SerializeField] private Button gameSpeedButton;
    [SerializeField] private Button startWaveButton;

    [SerializeField] private GameObject towerPanel;
    [SerializeField] private GameObject towerCardPrefab;
    [SerializeField] private Transform cardsContainer;

    [SerializeField] private TowerData[] _towers;
    // private List<GameObject> activeCards = new List<GameObject>();

    private Platform _currentPlatform;

    private bool _isPaused = false;
    private float _gameSpeed = 1f;
    private float _maxGameSpeed = 3f;
    public float GameSpeed => _gameSpeed;

    // private void OnValidate()
    // {
    //     if (_towers == null || _towers.Length == 0)
    //     {
    //         ResetTowerCards();
    //         FillTowerCards();
    //     }
    // }


    void OnEnable()
    {
        SpawnManager.OnWaveChanged += UpdateWaveText;
        TDGameManager.OnLivesChanged += UpdateLives;
        TDGameManager.OnGoldsChanged += UpdateGolds;
        Platform.OnPlatformClicked += OpenTowerPanel;
        TowerCard.OnTowerCardSelected += HandleTowerCardSelected;


    }

    void OnDisable()
    {
        SpawnManager.OnWaveChanged -= UpdateWaveText;
        TDGameManager.OnLivesChanged -= UpdateLives;
        TDGameManager.OnGoldsChanged -= UpdateGolds;
        Platform.OnPlatformClicked -= OpenTowerPanel;
        TowerCard.OnTowerCardSelected -= HandleTowerCardSelected;
    }

    void Start()
    {
        UpdateGameSpeedUI();
        HideAlert();

        gameSpeedButton.onClick.AddListener(OnGameSpeedButtonClicked);
        startWaveButton.onClick.AddListener(OnStartNewWave);
    }

    private void UpdateWaveText(int waveIndex)
    {
        waveText.text = "Wave " + (waveIndex + 1);
        UpdateStartWaveButton();
    }

    public void UpdateLives(int lives)
    {
        livesText.text = "Lives: " + lives;
    }

    public void UpdateGolds(int gold)
    {
        goldText.text = "Gold: " + gold;
    }

    public void UpdateGameSpeedUI()
    {
        gameSpeedButton.GetComponentInChildren<TMP_Text>().text = "x " + _gameSpeed;
    }

    public void UpdateStartWaveButton()
    {
        startWaveButton.interactable = !SpawnManager.Instance.ActiveWave;
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
    }

    private void HideAlert()
    {
        alertText.gameObject.SetActive(false);
    }

    private void ResetTowerCards()
    {
        // foreach (var card in activeCards)
        // {
        //     Destroy(card);
        // }
        // activeCards.Clear();
        foreach (Transform child in cardsContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void FillTowerCards()
    {
        foreach (var data in _towers)
        {
            GameObject card = Instantiate(towerCardPrefab, cardsContainer);
            TowerCard towerCard = card.GetComponent<TowerCard>();
            towerCard.Initialize(data);
            // activeCards.Add(card);
        }
    }
    private void PopulateTowerCards()
    {
        ResetTowerCards();
        FillTowerCards();
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

    public void SetGameSpeed(float speed)
    {
        _gameSpeed = Mathf.Clamp(speed, 1f, _maxGameSpeed);
        TDGameManager.Instance.SetTimeScale(_gameSpeed);
        UpdateGameSpeedUI();
    }

    public void OnGameSpeedButtonClicked()
    {
        _gameSpeed = (_gameSpeed + 1) % (_maxGameSpeed + 1);
        SetGameSpeed(_gameSpeed);
    }

    public void TogglePause()
    {
        _isPaused = !_isPaused;
        TDGameManager.Instance.SetTimeScale(_isPaused ? 0f : _gameSpeed);
    }

    public void OnStartNewWave()
    {
        SpawnManager.Instance.StartNewWave();
        UpdateStartWaveButton();
    }

    public void RestartGame()
    {
        // TDGameManager.Instance.RestartLevel();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
