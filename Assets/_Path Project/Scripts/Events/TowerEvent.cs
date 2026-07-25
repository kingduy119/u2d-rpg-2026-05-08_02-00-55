using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public static class TowerEvent
    {
        public static event Action<Ability> OnAbilitySelect;
        public static event Action OnTowerUpdateSelect;

        public static List<Ability> TowerAbilities = new();
        public static TowerAbility BonusAbility = new();

        public static void RaiseAbilitySelect(Ability ability)
        {
            ability.Apply(BonusAbility);
            TowerAbilities.Add(ability);

            Debug.Log($@"
            ShootRange: {BonusAbility.ShootRange}
            ShootInterval: {BonusAbility.ShootInterval}
            PhysicDamage: {BonusAbility.PhysicDamage}
            ");
            OnAbilitySelect?.Invoke(ability);
        }

        public static void TowerUpdateSelect()
        {
            UIManager.Instance.ShowTowerUpdateSelect();
        }

        public static void Log(string message)
        {
            Debug.Log(message);
        }
    }

}