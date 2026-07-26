using System;
using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "TowerOptionAbility", menuName = "Game TD/Abilities/TowerOptionAbility")]
    public class TowerOptionAbility : Ability
    {
        public TowerAbility TowerAbility;

        public override void Apply(GameObject go = null)
        {
            if (go != null && go.TryGetComponent<Tower>(out var entity))
            {
                entity.Ability += TowerAbility;
            }
        }
    }
}