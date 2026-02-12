using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PermanentUpgradeCardSpace;
using GhostCardSpace;

public class PermanentUpgradeManager : MonoBehaviour
{
    [SerializeField] private GhostCard[] ghostCardData;
    [SerializeField] private PermanentUpgradeCard[] availableUpgrades;

    // Tableaux pour stocker les stats améliorées de chaque ghost après application des upgrades
    //Base
    public int[] upgradedPoints { get; private set; }
    public float[] upgradedBaseSpeed { get; private set; }
    public float[] upgradedBaseSpeedMultiplier { get; private set; }
    //Chase 
    public int[] upgradedChaseDuration { get; private set; }
    public float[] upgradedChaseSpeedMultiplier { get; private set; }
    //Respawn
    public int[] upgradedRespawnDuration { get; private set; }
    //Scatter
    public float[] upgradedScatterSpeedMultiplier { get; private set; }
    public int[] upgradedScatterDuration { get; private set; }
    //Frightened 
    public float[] upgradedFrightenedSpeedMultiplier { get; private set; }
    public int[] upgradedFrightenedDuration { get; private set; }

    void Awake()
    {
        if (ghostCardData == null || ghostCardData.Length == 0)
        {
            Debug.LogError("PermanentUpgradeManager: ghostCardData is not assigned or empty! Please assign at least one ghostCardData in the Inspector.");
            return;
        }

        if (availableUpgrades == null || availableUpgrades.Length == 0)
        {
            Debug.LogError("PermanentUpgradeManager: availableUpgrades is not assigned or empty! Please assign at least one availableUpgrade in the Inspector.");
            return;
        }

        InitializeUpgradedArrays();
        CalculatePermanentUpgrades();
    }

    void InitializeUpgradedArrays()
    {
        int ghostCount = ghostCardData.Length;
        
        upgradedPoints = new int[ghostCount];
        upgradedBaseSpeed = new float[ghostCount];
        upgradedBaseSpeedMultiplier = new float[ghostCount];
        upgradedChaseDuration = new int[ghostCount];
        upgradedChaseSpeedMultiplier = new float[ghostCount];
        upgradedRespawnDuration = new int[ghostCount];
        upgradedScatterSpeedMultiplier = new float[ghostCount];
        upgradedScatterDuration = new int[ghostCount];
        upgradedFrightenedSpeedMultiplier = new float[ghostCount];
        upgradedFrightenedDuration = new int[ghostCount];
        
        // Initialiser avec les stats de base des ghosts
        for (int i = 0; i < ghostCount; i++)
        {
            upgradedPoints[i] = ghostCardData[i].points;
            upgradedBaseSpeed[i] = ghostCardData[i].baseSpeed;
            upgradedBaseSpeedMultiplier[i] = ghostCardData[i].baseSpeedMultiplier;
            upgradedChaseDuration[i] = ghostCardData[i].chaseDuration;
            upgradedChaseSpeedMultiplier[i] = ghostCardData[i].chaseSpeedMultiplier;
            upgradedRespawnDuration[i] = ghostCardData[i].respawnDuration;
            upgradedScatterSpeedMultiplier[i] = ghostCardData[i].scatterSpeedMultiplier;
            upgradedScatterDuration[i] = ghostCardData[i].scatterDuration;
            upgradedFrightenedSpeedMultiplier[i] = ghostCardData[i].frightenedSpeedMultiplier;
            upgradedFrightenedDuration[i] = ghostCardData[i].frightenedDuration;
        }
    }
    
    // Calculer et appliquer les bonus cumulés de toutes les upgrades
    void CalculatePermanentUpgrades() 
    { 
        // Variables locales pour le cumul des bonus
        int totalPointsBonus = 0;
        float totalBaseSpeedBonus = 0f;
        float totalBaseSpeedMultiplierBonus = 0f;
        int totalChaseDurationBonus = 0;
        float totalChaseSpeedMultiplierBonus = 0f;
        int totalRespawnDurationBonus = 0;
        float totalScatterSpeedMultiplierBonus = 0f;
        int totalScatterDurationBonus = 0;
        float totalFrightenedSpeedMultiplierBonus = 0f;
        int totalFrightenedDurationBonus = 0;

        // Additionner toutes les upgrades
        for (int i = 0; i < availableUpgrades.Length; i++)
        {
            PermanentUpgradeCard upgrade = availableUpgrades[i];
            
            totalPointsBonus -= upgrade.pointsDecrease;
            totalBaseSpeedBonus += upgrade.baseSpeedIncrease;
            totalBaseSpeedMultiplierBonus += upgrade.baseSpeedMultiplierIncrease;
            totalChaseDurationBonus += upgrade.chaseDurationIncrease;
            totalChaseSpeedMultiplierBonus += upgrade.chaseSpeedMultiplierIncrease;
            totalRespawnDurationBonus -= upgrade.respawnDurationDecrease;
            totalScatterSpeedMultiplierBonus += upgrade.scatterSpeedMultiplierIncrease;
            totalScatterDurationBonus += upgrade.scatterDurationIncrease;
            totalFrightenedSpeedMultiplierBonus += upgrade.frightenedSpeedMultiplierIncrease;
            totalFrightenedDurationBonus -= upgrade.frightenedDurationDecrease;
        }

        // Appliquer les bonus cumulés à chaque ghost
        for (int i = 0; i < ghostCardData.Length; i++)
        {
            upgradedPoints[i] += totalPointsBonus;
            upgradedBaseSpeed[i] += totalBaseSpeedBonus;
            upgradedBaseSpeedMultiplier[i] += totalBaseSpeedMultiplierBonus;
            upgradedChaseDuration[i] += totalChaseDurationBonus;
            upgradedChaseSpeedMultiplier[i] += totalChaseSpeedMultiplierBonus;
            upgradedRespawnDuration[i] += totalRespawnDurationBonus;
            upgradedScatterSpeedMultiplier[i] += totalScatterSpeedMultiplierBonus;
            upgradedScatterDuration[i] += totalScatterDurationBonus;
            upgradedFrightenedSpeedMultiplier[i] += totalFrightenedSpeedMultiplierBonus;
            upgradedFrightenedDuration[i] += totalFrightenedDurationBonus;
        }
    }

    public GhostCard[] GetGhostCardData()
    {
        return ghostCardData;
    }
}
