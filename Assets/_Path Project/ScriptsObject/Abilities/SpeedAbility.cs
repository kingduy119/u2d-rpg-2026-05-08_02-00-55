

using UnityEngine;

namespace TDGame
{
    [CreateAssetMenu(fileName = "SpeedAbility", menuName = "Game TD/Abilities/SpeedAbility")]
    public class SpeedAbility : Ability
    {
        [SerializeField] private int _attackSpeed;
        [SerializeField] private int _runSpeed;

        public override void Apply(GameObject go = null)
        {
            if (go != null && go.TryGetComponent<Tower>(out var entity))
            {
                entity.Ability.ShootInterval += _attackSpeed;
                //  entity.Ability._runSpeed += _runSpeed;
            }
        }

        public override void Apply(TowerAbility ability)
        {
            ability.ShootInterval -= _attackSpeed;
        }
    }
}