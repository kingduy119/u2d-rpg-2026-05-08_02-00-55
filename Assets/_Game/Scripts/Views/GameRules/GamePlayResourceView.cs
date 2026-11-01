

using UnityEngine;

public class GamePlayResourceView : MonoBehaviour
{
    [SerializeField] ItemView _food;
    [SerializeField] ItemView _gold;
    [SerializeField] ItemView _wood;

    public void Init(Resources resources)
    {
        _food.Value = resources.Food;
        _gold.Value = resources.Gold;
        _wood.Value = resources.Wood;
    }
}