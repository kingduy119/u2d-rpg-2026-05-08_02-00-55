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
            TowerAbilities.Add(ability);
            OnAbilitySelect?.Invoke(ability);
        }

        public static void TowerUpdateSelect()
        {
            OnTowerUpdateSelect?.Invoke();
        }

        public static void Log(string message)
        {
            Debug.Log(message);
        }
    }

}