using UnityEngine;


namespace TDGame
{
    public class TowerSelectUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_prefab;


        private void Awake()
        {
            Refresh();
        }

        private void Refresh()
        {
            foreach (Transform child in gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (var data in GameManager.Instance.Towers)
            {
                GameObject go = Instantiate(m_prefab, transform);
                TowerSelectCard card = go.GetComponent<TowerSelectCard>();
                card.Initialize(data);
            }
        }
    }

}