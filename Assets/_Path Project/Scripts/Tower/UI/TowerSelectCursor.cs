

using UnityEngine;
using UnityEngine.UI;

namespace TDGame
{
    public class TowerSelectCursor : MonoBehaviour
    {
        [SerializeField] private Button _sellButton;
        [SerializeField] private Button _detailButton;
        [SerializeField] private Button _updateButton;

        private void OnEnable()
        {
            _sellButton.onClick.AddListener(TowerSell);
            _detailButton.onClick.AddListener(TowerDetail);
            _updateButton.onClick.AddListener(TowerUpdate);
        }

        private void OnDisable()
        {
            _sellButton.onClick.RemoveListener(TowerSell);
            _detailButton.onClick.RemoveListener(TowerDetail);
            _updateButton.onClick.RemoveListener(TowerUpdate);
        }

        private void TowerSell()
        {
            Debug.Log("TowerSell");
        }
        private void TowerDetail()
        {
            Debug.Log("TowerDetail");
        }
        private void TowerUpdate()
        {
            Debug.Log("TowerUpdate");
        }

        public void Activate() => gameObject.SetActive(true);
        public void Deactivate() => gameObject.SetActive(false);
    }
}