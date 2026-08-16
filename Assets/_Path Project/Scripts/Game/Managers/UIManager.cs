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


        AssetLoader TowerSelectLoader;
        GameObject _TowerSelectCursorUI;
        public GameObject TowerSelectCursorUI
        {
            get
            {
                if (_TowerSelectCursorUI == null) _TowerSelectCursorUI = TowerSelectLoader.Instantiate(transform);
                return _TowerSelectCursorUI;
            }
        }


        protected override void Awake()
        {
            base.Awake();
            Coroutines.Initialize(this);
            TowerSelectLoader = new("Tower/TowerSelectCursorUI", true);
        }


        void OnEnable()
        {
            GamePlayEvent.ShowSelectCursor += OnShowSelectCursor;
            GamePlayEvent.HideSelectCursor += OnHideSelectCursor;
        }

        void OnDisable()
        {
            GamePlayEvent.ShowSelectCursor -= OnShowSelectCursor;
            GamePlayEvent.HideSelectCursor -= OnHideSelectCursor;
        }

        private void OnHideSelectCursor() => TowerSelectCursorUI.SetActive(false);
        private void OnShowSelectCursor(Vector3 position)
        {
            TowerSelectCursorUI.transform.position = position;
            TowerSelectCursorUI.SetActive(true);
        }
    }
}