

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class AddressableLabels
{
    public const string Default = "default";
    public const string Inventory = "inventory_pack";
    public const string Character = "character_pack";
}

public class SceneLoadingView : MonoBehaviour
{
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private TMP_Text _progressText;
    [SerializeField] private TMP_Text _dataText;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _progressText.text = "";
        _dataText.text = "";
    }

    public void UpdateProgress(float percent, long downloaded, long total)
    {
        _progressSlider.value = percent;
        _progressText.text = $"{percent * 100:F2}%";
        _dataText.text = $"{SizeFormatter.Format(downloaded)} / {SizeFormatter.Format(total)}";

    }
}