

using UnityEngine;

namespace TDGame
{
    public class ButtonTowerDetail : ClickEvent
    {
        public override void RaiseEvent(GameObject tower)
        {
            if (tower != null && tower.TryGetComponent<TowerCombat>(out var combat))
            {
                // Debug.Log($@"
                // ShootRange: {tower.TowerSO.Ability.ShootRange}
                // ShootInterval: {tower.TowerSO.Ability.ShootInterval}
                // PhysicDamage: {tower.TowerSO.Ability.PhysicDamage}
                // ");
            }
        }
    }

}