using System.Collections.Generic;
using UnityEngine;

public class GameDefenseRuleView : MonoBehaviour
{
    public List<TargetController> enemies;
    public List<TargetController> players;

    [Header("Enemy Setting")]
    [SerializeField] LayerMask EnemyTargetLayer;
    [SerializeField] Transform _enemyDefaultTarget;


    [Header("Player Setting")]
    [SerializeField] LayerMask PlayerTargetLayer;
    [SerializeField] Transform _playerDefaultTarget;

    void Start()
    {
        if (enemies.Count > 0)
        {
            foreach (var target in enemies) target.Init(EnemyTargetLayer, _enemyDefaultTarget);
        }

        if (players.Count > 0)
        {
            foreach (var target in players) target.Init(PlayerTargetLayer, _playerDefaultTarget);
        }
    }
}
