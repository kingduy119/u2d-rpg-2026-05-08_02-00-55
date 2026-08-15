using UnityEngine;

namespace TDGame
{
    public class UIManager : PersistentSingleton<UIManager>
    {

        [SerializeField] private TowerAbilityOptionsUI _TowerAbilityOptionsPrefab;

        [SerializeField] private GameObject _TowerPlaceCursorPrefab;
        private GameObject _TowerPlaceCursor;
        public GameObject TowerPlaceCursor => Lazy.Load(ref _TowerPlaceCursor, _TowerPlaceCursorPrefab, transform);

        [SerializeField] private GameObject _TowerSelectCursorPrefab;
        private GameObject _TowerSelectCursor;
        public GameObject TowerSelectCursor => Lazy.Load(ref _TowerSelectCursor, _TowerSelectCursorPrefab, transform);

        protected override void Awake()
        {
            base.Awake();
        }
    }
}