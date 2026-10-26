using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        // ButtonEvents.GamePlayEvents += GamePlayEvents;
    }

    public void OnDisable()
    {
        // ButtonEvents.GamePlayEvents -= GamePlayEvents;
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
    }

    void Update()
    {

        // if (!Started) return;

        // if (enemies.Count <= 0)
        // {
        //     // Debug.Log("Player Win");
        //     Started = false;
        // }
        // else if (players.Count <= 0)
        // {
        //     Debug.Log("Player Lose");
        //     _hearts--;
        //     Started = false;

        //     if (_hearts <= 0) Debug.Log("Game Over");
    }





}



public class GamePlayProfile
{

}