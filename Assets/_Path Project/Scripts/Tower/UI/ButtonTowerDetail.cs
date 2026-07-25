

using UnityEngine;

namespace TDGame
{
    public class ButtonTowerDetail : ClickEvent
    {
        public override void RaiseEvent(GameObject go)
        {
            if (go != null && go.TryGetComponent<Tower>(out var tower))
            {
                Debug.Log($@"
                ShootRange: {tower.TowerSO.Ability.ShootRange}
                ShootInterval: {tower.TowerSO.Ability.ShootInterval}
                PhysicDamage: {tower.TowerSO.Ability.PhysicDamage}
                ");
            }
        }
    }

}