using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestRewardSlot : MonoBehaviour
{
    public Image image;
    public TMP_Text quantityText;

    public void DisplayReward(Sprite sprite, int quantity)
    {
        image.sprite = sprite;
        quantityText.text = $"{quantity}";
    }
}
