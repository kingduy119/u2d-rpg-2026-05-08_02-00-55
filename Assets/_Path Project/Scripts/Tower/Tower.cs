using UnityEngine;
using System.Collections.Generic;
using System;

namespace TDGame
{
    public class Tower : TowerBase
    {

    }
    // [RequireComponent(typeof(Tower_Combat))]
    // public class Tower : MonoBehaviour
    // {
    //     [SerializeField] private TowerSO m_data;
    //     [SerializeField] private bool m_showDraw;
    //     [SerializeField] private SpriteRenderer m_render;

    //     Tower_Combat m_combat;


    //     private void OnValidate()
    //     {
    //         if (!m_data) return;
    //         m_render.sprite = m_data.sprite;
    //     }
    //     private void Awake()
    //     {
    //         m_combat = GetComponent<Tower_Combat>();
    //     }

    //     private void Start()
    //     {
    //         m_combat.Init(m_data);
    //     }

    //     private void OnDrawGizmos()
    //     {
    //         if (m_showDraw)
    //         {
    //             Gizmos.color = Color.red;
    //             Gizmos.DrawWireSphere(transform.position, m_data.range);
    //         }
    //     }
    // }
}