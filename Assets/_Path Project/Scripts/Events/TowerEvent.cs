using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public static class TowerEvent
    {
        // Buy and build tower:
        public static Action OnAcceptBuild;
        public static Action OnCancelBuild;
        public static Action<TowerSO> OnTowerCardSelect;
        public static Action<TowerBase> OnTowerPlace;

        public static Action<bool> OnAcceptBuildResult;

        // Select Tower:
        public static event Action<Ability> OnAbilitySelect;
        public static Action OnSelectUpdateTower;

        public static List<Ability> TowerAbilities = new();
        public static TowerAbility BonusAbility = new();

        public static void RaiseAbilitySelect(Ability ability)
        {
            TowerAbilities.Add(ability);
            OnAbilitySelect?.Invoke(ability);
        }

        public static void Log(string message)
        {
            Debug.Log(message);
        }
    }

}