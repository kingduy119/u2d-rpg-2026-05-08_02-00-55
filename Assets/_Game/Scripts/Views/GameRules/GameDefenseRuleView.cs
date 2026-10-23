using System.Collections.Generic;
using UnityEngine;

public class GameDefenseRuleView : MonoBehaviour
{
    public List<TargetController> enemies;
    public List<TargetController> players;

    [Header("Enemy Setting")]
    [SerializeField] string EnemyLayer;
    [SerializeField] LayerMask EnemyTargetLayer;
    [SerializeField] Transform _enemyDefaultTarget;


    [Header("Player Setting")]
    [SerializeField] string PlayerLayer;
    [SerializeField] LayerMask PlayerTargetLayer;
    [SerializeField] Transform _playerDefaultTarget;

    public void OnEnable()
    {
        ButtonEvents.GamePlayEvents += GamePlayEvents;
    }

    public void OnDisable()
    {
        ButtonEvents.GamePlayEvents -= GamePlayEvents;
    }

    void Start()
    {

        if (enemies.Count > 0)
        {
            foreach (var target in enemies)
                target.Init(EnemyLayer, EnemyTargetLayer, _enemyDefaultTarget);
        }

        if (players.Count > 0)
        {
            foreach (var target in players)
                target.Init(PlayerLayer, PlayerTargetLayer, _playerDefaultTarget);
        }
        // Time.timeScale = 0;
    }

    void Update()
    {

        if (!Started) return;

        if (enemies.Count <= 0)
        {
            Debug.Log("Player Win");
            Started = true;
        }
        else if (players.Count <= 0)
        {
            Debug.Log("Player Lose");
            _hearts--;
            if (_hearts <= 0) Debug.Log("Game Over");
            Started = true;
        }

    }

    int _hearts = 3;
    public static bool Started = false;
    private bool Paused = false;

    private void GamePlayEvents(string eventName)
    {
        switch (eventName)
        {
            case "StartGame":
                Started = true;
                // Time.timeScale = 1;
                break;
            case "ToggleSetting":
                Paused = !Paused;
                Time.timeScale = Paused ? 0 : 1;
                break;
            default:
                Debug.LogWarning($"Unknown event: {name}");
                break;
        }
    }

}
