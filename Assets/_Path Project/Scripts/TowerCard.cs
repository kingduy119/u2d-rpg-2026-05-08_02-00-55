using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour
{
    [SerializeField] private Image towerImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text costText;

    private TowerData _data;
    public static event Action<TowerData> OnTowerCardSelected;

    public void Initialize(TowerData data)
    {
        towerImage.sprite = data.sprite;
        nameText.text = data.towerName;
        costText.text = data.cost.ToString();
    }

    public void PlaceTower()
    {
        OnTowerCardSelected?.Invoke(_data);
    }
}
