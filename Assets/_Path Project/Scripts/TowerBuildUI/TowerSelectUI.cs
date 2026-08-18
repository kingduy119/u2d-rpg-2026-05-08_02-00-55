using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{

    public class TowerSelectUI : MonoBehaviour
    {
        // [SerializeField] private GameObject m_TowerCardPrefab;
        [SerializeField] private GameObject _CardList;
        [SerializeField] private AssetReference _CardRef;

        private AsyncOperationHandle<GameObject> _handle;
        private GameObject _cardPrefab;

        private TowerBoard TowerBoard => GameManager.Instance.TowerBoard;

        private void Start()
        {
            AsyncOperationHandle<GameObject> _handle = _CardRef.LoadAssetAsync<GameObject>();
            _handle.Completed += Handle_Completed;

        }

        private void Handle_Completed(AsyncOperationHandle<GameObject> handle)
        {

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"AssetReference {_CardRef.RuntimeKey} failed to load.");
                return;
            }

            _cardPrefab = handle.Result;
            RefreshUI();
        }


        private void RefreshUI()
        {
            foreach (Transform child in _CardList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var data in TowerBoard.Towers)
            {
                GameObject go = Instantiate(_cardPrefab, _CardList.transform);
                if (go.TryGetComponent<TowerSelectSlot>(out var card))
                {
                    card.Initialize(data);

                }
            }

        }

        private void OnDestroy()
        {
            if (_handle.IsValid())
            {
                _handle.Completed -= Handle_Completed;
                Addressables.Release(_handle);
            }
        }


    }
}