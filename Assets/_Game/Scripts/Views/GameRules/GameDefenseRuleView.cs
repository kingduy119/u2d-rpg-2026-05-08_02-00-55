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

    void Start()
    {
        if (enemies.Count > 0)
        {
            foreach (var target in enemies) target.Init(EnemyLayer, EnemyTargetLayer, _enemyDefaultTarget);
        }

        if (players.Count > 0)
        {
            foreach (var target in players) target.Init(PlayerLayer, PlayerTargetLayer, _playerDefaultTarget);
        }
    }


}
