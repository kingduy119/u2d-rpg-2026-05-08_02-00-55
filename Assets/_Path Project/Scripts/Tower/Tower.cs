using UnityEngine;
using System.Collections.Generic;
using System;

namespace TDGame
{
    [RequireComponent(typeof(Tower_Combat))]
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerData _data;

        Tower_Combat m_combat;


        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            m_combat = GetComponent<Tower_Combat>();
        }

    }

}