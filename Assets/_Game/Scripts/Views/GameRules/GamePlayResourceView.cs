

using UnityEngine;
using VContainer;

public class GamePlayResourceView : MonoBehaviour
{
    [SerializeField] ItemView _food;
    [SerializeField] ItemView _gold;
    [SerializeField] ItemView _wood;

    [Inject] readonly public ProfileSerice Profile;

    void Awake()
    {
        Debug.Log($"Profile: {Profile != null}");
    }

    void OnEnable()
    {
        if (Profile != null) Profile.gResources.OnChange += UpdateView;
    }
    void OnDisable()
    {
        if (Profile != null) Profile.gResources.OnChange -= UpdateView;
    }

    public void UpdateView(Resources resources)
    {
        if (resources == null) return;
        _food.Value = resources.Food;
        _gold.Value = resources.Gold;
        _wood.Value = resources.Wood;
    }
}