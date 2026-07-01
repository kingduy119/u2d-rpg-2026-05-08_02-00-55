using UnityEngine;
using System.Collections.Generic;
using System;

namespace TDGame
{
    [RequireComponent(typeof(Tower_Combat))]
    public class Tower : MonoBehaviour
    {
        [SerializeField] private TowerSO _data;
        [SerializeField] private bool m_showDraw;

        Tower_Combat m_combat;


        private void Awake()
        {
            m_combat = GetComponent<Tower_Combat>();
        }

        private void Start()
        {
            m_combat.Init(_data);
        }

        private void OnDrawGizmos()
        {
            if (m_showDraw)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _data.range);
            }
        }
    }
}