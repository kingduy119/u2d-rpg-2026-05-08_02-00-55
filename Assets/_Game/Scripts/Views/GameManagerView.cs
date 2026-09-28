
using UnityEngine;
using Characters;
using Weapons;

namespace Game
{
    public class GameManagerView : PersistentSingleton<GameManagerView>
    {
        public ProjectSO projectSO;
        public KeyBoardHandler KeyBoard;
        public readonly FactoryManager Factory = new();

        protected override void Awake()
        {
            base.Awake();
            Factory.LoadAssets();
        }

        private void OnEnable()
        {
            CterEvent.Shoot += Character_Shoot;
        }
        private void OnDisable()
        {
            CterEvent.Shoot -= Character_Shoot;
        }

        private void Character_Shoot(Character cter)
        {
            Vector2 direction = (cter.Target.position - cter.transform.position).normalized;
            var go = Factory.GetPrefab(projectSO);
            if (go) go.Launch(cter.transform, direction);
        }
    }

}