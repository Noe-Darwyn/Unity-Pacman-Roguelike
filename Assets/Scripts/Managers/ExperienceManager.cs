using UnityEngine;
using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] AnimationCurve experienceCurve;
    
    private int currentLevel;
    public int totalExperience;
    private int previousLevelExperience; 
    private int nextLevelExperience;

    [Header("Interface Elements")]  
    [SerializeField] private Text levelText;
    [SerializeField] private Text experienceText;
    [SerializeField] private Image experiencefill;

    public void SetExperience()
    {
        currentLevel = 0;
        totalExperience = 0;
        previousLevelExperience = 0;
        nextLevelExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        UpdateLevel();
    }

    public void AddExperience(int amount)
    {
        totalExperience += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    void CheckForLevelUp()
    {
        if(totalExperience >= nextLevelExperience)
        {
            currentLevel++;
            UpdateLevel(); 
        }
    }

    void UpdateLevel()
    {
        previousLevelExperience = (int)experienceCurve.Evaluate(currentLevel);
        nextLevelExperience = (int)experienceCurve.Evaluate(currentLevel + 1);
        // Donne le choix de selectionner une upgrade

        UpdateInterface();
    }

    void UpdateInterface()
    {
        int start = totalExperience - previousLevelExperience;
        int end = nextLevelExperience - previousLevelExperience;

        levelText.text = currentLevel.ToString() + " LVL";
        experienceText.text = start + "XP / " + end + "XP";
        experiencefill.fillAmount = (float)start / end;
    }
}
