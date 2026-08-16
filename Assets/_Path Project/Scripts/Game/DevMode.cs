using UnityEngine;
using UnityEngine.AddressableAssets;

namespace TDGame
{
    public class DevMode : PersistentSingleton<DevMode>
    {
        [SerializeField] private GameObject Camera;
        private bool Loaded = false;

        protected override void Awake()
        {
            base.Awake();
            Camera.SetActive(false);
        }

        void Start()
        {
            if (Loaded) return;

            Addressables.LoadSceneAsync("Scene/TD_Boostrap", activateOnLoad: true);
            Loaded = true;
        }
    }

}