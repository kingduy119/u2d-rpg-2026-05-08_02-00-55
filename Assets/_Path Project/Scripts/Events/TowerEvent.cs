using System;
using UnityEngine;

namespace TDGame
{
    public static class TowerEvent
    {
        public static event Action<Ability> OnAbilitySelect;
        public static event Action OnTowerUpdateSelect;

        public static void RaiseAbilitySelect(Ability ability)
        {
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