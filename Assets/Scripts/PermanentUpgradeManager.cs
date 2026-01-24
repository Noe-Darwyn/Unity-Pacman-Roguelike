using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PermanentUpgradeCardSpace;
using GhostCardSpace;

public class PermanentUpgradeManager : MonoBehaviour
{
    [SerializeField] private GhostCard[] ghostCardData;
    [SerializeField] private List<PermanentUpgradeCard> availableUpgrades;

    // Upgraded stats arrays
    
    public int[] upgradedPoints { get; private set; }

    public int[] upgradedBaseSpeed { get; private set; }
    public int[] upgradedBaseSpeedMultiplier { get; private set; }  
    public int[] upgradedChaseDuration { get; private set; }
    public int[] upgradedChaseSpeedMultiplier { get; private set; }

    public int[] upgradedRespawnDuration { get; private set; }
    
    public int[] upgradedScatterSpeedMultiplier { get; private set; }
    public int[] upgradedScatterDuration { get; private set; }

    public int[] upgradedFrightenedSpeedMultiplier { get; private set; }
    public int[] upgradedFrightenedDuration { get; private set; }
    

    void Awake()
    {
        if (ghostCardData == null || ghostCardData.Length == 0)
        {
            Debug.LogError("PermanentUpgradeManager: ghostCardData is not assigned or empty! Please assign at least one ghostCardData in the Inspector.");
            return;
        }

        if (availableUpgrades == null || availableUpgrades.Count == 0)
        {
            Debug.LogError("PermanentUpgradeManager: availableUpgrades is not assigned or empty! Please assign at least one availableUpgrade in the Inspector.");
            return;
        }

        if (ghostCardData.Length != availableUpgrades.Count)
        {
            Debug.LogError($"PermanentUpgradeManager: Array length mismatch! ghostCardData has {ghostCardData.Length} elements, but availableUpgrades has {availableUpgrades.Count} elements. They must be equal.");
            return;
        }

        InitializePermanentUpgradeManager();
        //if you have no upgrades acquired yet, all arrays will be 0 until an upgrade is applied
        ApplyUpgradeEffects(availableUpgrades[0]);
    }

    public void InitializePermanentUpgradeManager()
    {
        // Initialize arrays based on ghost count
        upgradedPoints = new int[ghostCardData.Length];

        upgradedBaseSpeed = new int[ghostCardData.Length];
        upgradedBaseSpeedMultiplier = new int[ghostCardData.Length];

        upgradedChaseDuration = new int[ghostCardData.Length];
        upgradedChaseSpeedMultiplier = new int[ghostCardData.Length];

        upgradedRespawnDuration = new int[ghostCardData.Length];
        
        upgradedScatterSpeedMultiplier = new int[ghostCardData.Length];
        upgradedScatterDuration = new int[ghostCardData.Length];

        upgradedFrightenedSpeedMultiplier = new int[ghostCardData.Length];
        upgradedFrightenedDuration = new int[ghostCardData.Length];

        // Initialize upgraded stats with base ghost stats
        for (int i = 0; i < ghostCardData.Length; i++)
        {
            upgradedPoints[i] = ghostCardData[i].points;

            upgradedBaseSpeed[i] = ghostCardData[i].baseSpeed;
            upgradedBaseSpeedMultiplier[i] = ghostCardData[i].baseSpeedMultiplier;

            upgradedChaseDuration[i] = ghostCardData[i].chaseDuration;
            upgradedChaseSpeedMultiplier[i] = ghostCardData[i].chaseSpeedMultiplier;

            upgradedRespawnDuration[i] = ghostCardData[i].respawnDuration;

            upgradedScatterSpeedMultiplier[i] = ghostCardData[i].scatterSpeedMultiplier;
            upgradedScatterDuration[i] = ghostCardData[i].scatterDuration;

            upgradedFrightenedDuration[i] = ghostCardData[i].frightenedDuration;
            upgradedFrightenedSpeedMultiplier[i] = ghostCardData[i].frightenedSpeedMultiplier;
        }

    }

    private void ApplyUpgradeEffects(PermanentUpgradeCard upgrade)
    {
        Debug.Log($"Applying effects of upgrade: {upgrade.upgradeName}");

        for (int i = 0; i < ghostCardData.Length; i++)
        {
            upgradedPoints[i] = ghostCardData[i].points - upgrade.pointsDecrease;
            
            upgradedBaseSpeed[i] = ghostCardData[i].baseSpeed + upgrade.baseSpeedIncrease;
            upgradedBaseSpeedMultiplier[i] = ghostCardData[i].baseSpeedMultiplier + upgrade.baseSpeedMultiplierIncrease;

            upgradedChaseDuration[i] = ghostCardData[i].chaseDuration + upgrade.chaseDurationIncrease;
            upgradedChaseSpeedMultiplier[i] = ghostCardData[i].chaseSpeedMultiplier + upgrade.chaseSpeedMultiplierIncrease;

            upgradedRespawnDuration[i] = ghostCardData[i].respawnDuration - upgrade.respawnDurationDecrease;

            upgradedScatterDuration[i] = ghostCardData[i].scatterDuration + upgrade.scatterDurationIncrease;
            upgradedScatterSpeedMultiplier[i] = ghostCardData[i].scatterSpeedMultiplier + upgrade.scatterSpeedMultiplierIncrease;
            
            upgradedFrightenedDuration[i] = ghostCardData[i].frightenedDuration - upgrade.frightenedDurationDecrease;
            upgradedFrightenedSpeedMultiplier[i] = ghostCardData[i].frightenedSpeedMultiplier + upgrade.frightenedSpeedMultiplierIncrease;
        }
    }

    public GhostCard[] GetGhostCardData()
    {
        return ghostCardData;
    }
}
