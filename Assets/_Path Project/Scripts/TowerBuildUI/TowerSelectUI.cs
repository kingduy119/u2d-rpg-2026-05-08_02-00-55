using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace TDGame
{

    public class TowerSelectUI : MonoBehaviour
    {
        [SerializeField] private GameObject _CardList;

        private TowerBoard TowerBoard => GameManager.Instance.TowerBoard;

        AssetLoader SlotLoader;
        List<TowerSelectSlot> AllSlots = new();

        void OnEnable()
        {
            GamePlayEvent.ResponseUpdateUI += GamePlayEvent_ResponseUpdateUI;
        }

        void OnDisable()
        {
            GamePlayEvent.ResponseUpdateUI -= GamePlayEvent_ResponseUpdateUI;
        }

        private void Start()
        {
            SlotLoader = new("Tower/TowerSelectSlotUI", OnCompleted);
        }

        private void OnCompleted(GameObject prefab)
        {
            RefreshUI(prefab);
            GamePlayEvent.RequestUpdateUI?.Invoke();
        }

        private void RefreshUI(GameObject prefab)
        {
            foreach (Transform child in _CardList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var data in TowerBoard.Towers)
            {
                GameObject go = Instantiate(prefab, _CardList.transform);
                if (go.TryGetComponent<TowerSelectSlot>(out var slot))
                {
                    slot.Initialize(data);
                    AllSlots.Add(slot);
                }
            }
        }

        private void GamePlayEvent_ResponseUpdateUI(GamePlayState state)
        {
            foreach (var slot in AllSlots)
            {
                slot.CheckActivve(state);
            }
        }

        private void OnDestroy()
        {
            SlotLoader.Release();
        }
    }
}