using System;
using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public static class TowerEvent
    {
        // Buy and build tower:
        public static Action AcceptBuild;
        public static Action OnCancelBuild;
        public static Action<TowerSO> TowerCardClick;
        public static Action<TowerSO> TowerBuildSlotClick;
        public static Action<TowerSO> ShowTowerBuild;
        public static Action<TowerBase> TowerPlace;

        // Select Tower:
        public static Action OnSellTower;
        public static Action OnDetailTower;
        public static Action OnSelectUpdateTower;

        public static event Action<Ability> OnAbilitySelect;

        public static List<Ability> TowerAbilities = new();
        public static TowerAbility BonusAbility = new();

        public static void RaiseAbilitySelect(Ability ability)
        {
            TowerAbilities.Add(ability);
            OnAbilitySelect?.Invoke(ability);
        }
    }

}