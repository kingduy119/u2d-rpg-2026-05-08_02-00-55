using System.Collections.Generic;
using UnityEngine;

namespace TDGame
{
    public class Object_Pool : MonoBehaviour
    {
        [SerializeField] private int _poolSize = 3;
        [SerializeField] private GameObject _prefab;
        private List<GameObject> _pool;

        void Start()
        {
            _pool = new List<GameObject>();
            for (int i = 0; i < _poolSize; i++)
            {
                CreateNewObject();
            }
        }

        private GameObject CreateNewObject()
        {
            GameObject obj = Instantiate(_prefab, transform);
            obj.SetActive(false);
            _pool.Add(obj);
            return obj;
        }

        public GameObject GetObject()
        {
            foreach (GameObject obj in _pool)
            {
                if (!obj.activeSelf)
                {
                    return obj;
                }
            }
            return CreateNewObject();
        }
    }

}