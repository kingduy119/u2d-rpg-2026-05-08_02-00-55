using System;
using UnityEngine;

namespace TDGame
{
    public static class TowerEvent
    {
        public static event Action<Ability> OnAbilitySelect;

        public static void RaiseAbilitySelect(Ability ability)
        {
            OnAbilitySelect?.Invoke(ability);
        }


        public static void RaisePointUpOnTower(GameObject tower)
        {
            Debug.Log("RaiseTowerPointerUpEvent");
        }

        public static void Log(string message)
        {
            Debug.Log(message);
        }
    }

}