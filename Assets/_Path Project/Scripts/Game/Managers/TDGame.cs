using UnityEngine;

namespace TDGame
{
    public class TDGame<T> : PersistentSingleton<T>
        where T : MonoBehaviour
    {
        [SerializeField] protected LevelManager _levelManagerPrefab;
        [SerializeField] protected FactoryManager _factoryManagerPrefab;
        [SerializeField] protected TowerBoard _towerBoardPrefab;
        [SerializeField] protected AudioController AudioController;

        protected LevelManager _levelManager;
        protected FactoryManager _factoryManager;
        protected TowerBoard _towerBoard;

        // public LevelManager LevelManager => LazyLoad(ref _levelManager, _levelManagerPrefab, gameObject.transform);
        // public FactoryManager FactoryManager => LazyLoad(ref _factoryManager, _factoryManagerPrefab, gameObject.transform);
        // public TowerBoard TowerBoard => LazyLoad(ref _towerBoard, _towerBoardPrefab, gameObject.transform);

        // public AudioController Audio => m_Audio;
        // public GameState GameState;

        // protected override void Awake()
        // {
        //     base.Awake();
        //     GameState = new();

        //     var audio = transform.Find("AudioController");
        //     if (audio && audio.TryGetComponent<AudioController>(out var instance))
        //     {
        //         m_Audio = instance;
        //         m_Audio.PlayMainMenuMusic();
        //         GameEvent.Audio = m_Audio;
        //     }
        // }


        // protected T LazyLoad<T>(ref T instance, T prefab, Transform transform = null)
        //     where T : Object
        // {
        //     if (instance == null)
        //         instance = Instantiate(prefab, transform);

        //     return instance;
        // }

    }
}
