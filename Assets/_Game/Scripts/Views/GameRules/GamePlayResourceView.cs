

using UnityEngine;
using VContainer;

public class GamePlayResourceView : MonoBehaviour
{
    [SerializeField] ItemView _food;
    [SerializeField] ItemView _gold;
    [SerializeField] ItemView _wood;

    [Inject] readonly public ProfileSerice _ProfileService;

    void Awake()
    {
        Debug.Log($"_ProfileService: {_ProfileService != null}");
    }

    void OnEnable()
    {
        if (_ProfileService != null) _ProfileService.gResources.OnChange += UpdateView;
    }
    void OnDisable()
    {
        if (_ProfileService != null) _ProfileService.gResources.OnChange -= UpdateView;
    }

    public void UpdateView(Resources resources)
    {
        if (resources == null) return;
        if (_gold != null) _gold.Value = resources.Gold;
        if (_food != null) _food.Value = resources.Food;
        if (_wood != null) _wood.Value = resources.Wood;
    }
}