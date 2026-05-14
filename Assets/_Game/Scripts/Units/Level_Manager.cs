using UnityEngine;
using UnityEngine.UI;

public class Level_Manager : MonoBehaviour
{
    public static Level_Manager Instance;

    public int level = 1;
    public int experience = 0;
    public int experienceToNextLevel = 100;

    public Slider expSlider;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        Enemy_Health.OnMonsterDeath += GainExperience;
    }
    void OnDisable()
    {
        Enemy_Health.OnMonsterDeath -= GainExperience;
    }

    void UpdateUI()
    {
        expSlider.value = experience;
        expSlider.maxValue = experienceToNextLevel;
    }

    public void GainExperience(int exp)
    {
        experience += exp;
        if (experience >= experienceToNextLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    void LevelUp()
    {
        level++;
        experience = 0;
        experienceToNextLevel += 50;
    }
}
