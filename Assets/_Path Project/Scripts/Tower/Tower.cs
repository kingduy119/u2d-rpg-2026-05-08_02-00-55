using UnityEngine;

namespace TDGame
{
    public class Tower : TowerBase
    {
        private TestFactory TestFactory => GameManager.Instance.FactoryManager.TestFactory;

        // private Transform _ShootRangeRadar;

        // protected override void Awake()
        // {
        //     base.Awake();
        //     _ShootRangeRadar = transform.Find("ShootRangeRadar");
        // }

        // protected override void OnValidate()
        // {
        //     base.OnValidate();
        //     ShowShootRange();
        // }

        // protected override void Start()
        // {
        //     base.Start();
        //     ShowShootRange();
        // }

        public override void TowerUP()
        {
            if (TowerSO.NextTowerLevel != null)
            {
                TowerBase tower = TestFactory.GetObject(TowerSO.NextTowerLevel);
                tower.gameObject.transform.position = transform.position;

                // TowerBase tower = TestFactory.GetObject(TowerSO.NextTowerLevel);
                // tower.gameObject.transform.position = transform.position;

                Deactivate();
            }
        }

        // private void ShowShootRange()
        // {
        //     if (_ShootRangeRadar != null && TowerSO != null)
        //     {
        //         float dimeter = TowerSO.Ability.ShootRange * 2f;
        //         _ShootRangeRadar.localScale = Vector3.one * dimeter;
        //     }
        // }
    }
}