using UnityEngine;

namespace TDGame
{
    public class UIManager : PersistentSingleton<UIManager>
    {

        // AssetLoader TowerBuildLoader;
        // GameObject _TowerBuildCursor;
        // public GameObject TowerBuildCursor
        // {
        //     get
        //     {
        //         if (_TowerBuildCursor == null) _TowerBuildCursor = TowerBuildLoader.Instantiate();
        //         return _TowerBuildCursor;
        //     }
        // }

        // AssetLoader TowerSelectLoader;
        // GameObject _TowerSelectCursor;
        // public GameObject TowerSelectCursor
        // {
        //     get
        //     {
        //         if (_TowerSelectCursor == null) _TowerSelectCursor = TowerSelectLoader.Instantiate();
        //         return _TowerSelectCursor;
        //     }
        // }




        protected override void Awake()
        {
            base.Awake();
            // Coroutines.Initialize(this);
            // TowerSelectLoader = new("Tower/TowerSelectCursor", true);
            // TowerBuildLoader = new("Tower/TowerBuiildCursor", true);
        }


        void OnEnable()
        {
            // GamePlayEvent.ShowSelectCursor += OnShowSelectCursor;
            // GamePlayEvent.HideSelectCursor += OnHideSelectCursor;
            // GamePlayEvent.ShowBuildCursor += OnShowBuildCursor;
            // GamePlayEvent.HideBuildCursor += OnHideBuildCursor;
        }

        void OnDisable()
        {
            // GamePlayEvent.ShowSelectCursor -= OnShowSelectCursor;
            // GamePlayEvent.HideSelectCursor -= OnHideSelectCursor;
            // GamePlayEvent.ShowBuildCursor -= OnShowBuildCursor;
            // GamePlayEvent.HideBuildCursor -= OnHideBuildCursor;
        }

        void OnDestroy()
        {

        }

        // private void OnHideSelectCursor() => TowerSelectCursor.SetActive(false);
        // private void OnShowSelectCursor(Vector3 position)
        // {
        //     TowerSelectCursor.transform.position = position;
        //     TowerSelectCursor.SetActive(true);
        // }

        // private void OnHideBuildCursor() => TowerBuildCursor.SetActive(false);
        // private void OnShowBuildCursor(Vector3 position)
        // {
        //     TowerBuildCursor.transform.position = position;
        //     TowerBuildCursor.SetActive(true);
        // }
    }
}