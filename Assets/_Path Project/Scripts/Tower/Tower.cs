using UnityEngine;

namespace TDGame
{
    public class Tower : TowerBase
    {
        private TestFactory TestFactory => GameManager.Instance.FactoryManager.TestFactory;

        protected override void Awake()
        {
            base.Awake();
        }

        public override void TowerUP()
        {
            Debug.Log("Tower.TowerUP");
            if (TowerSO.NextTowerLevel != null)
            {
                TowerBase tower = TestFactory.GetObject(TowerSO.NextTowerLevel);
                tower.gameObject.transform.position = transform.position;

                // TowerBase tower = TestFactory.GetObject(TowerSO.NextTowerLevel);
                // tower.gameObject.transform.position = transform.position;

                Deactivate();
            }
        }
    }
}