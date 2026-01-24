using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostCardSpace;

public class GhostBuilder : MonoBehaviour
{
    [SerializeField] private PermanentUpgradeManager upgradedGhostData;

    public Ghost[] ghosts;

    void Awake()
    {

        if (upgradedGhostData == null)
        {
            Debug.LogError("GhostBuilder: PermanentUpgradeManager instance is not found! Ensure that a PermanentUpgradeManager exists in the scene.");
            return;
        }

    } 
    
    void Start()
    {
        CreateGhosts();
        SetGhostStats();
    }

    void CreateGhosts()
    {
        
    }

    void SetGhostStats()
    {
        GhostCard[] ghostCardData = upgradedGhostData.GetGhostCardData();
        
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].initialBehaviorType = ghostCardData[i].initialBehaviorType;

            ghosts[i].points = upgradedGhostData.upgradedPoints[i];
            
            ghosts[i].movement.speed = upgradedGhostData.upgradedBaseSpeed[i];
            ghosts[i].movement.speedMultiplier = upgradedGhostData.upgradedBaseSpeedMultiplier[i];

            ghosts[i].chase.duration = upgradedGhostData.upgradedChaseDuration[i];
            ghosts[i].chase.chaseSpeedMultiplier = upgradedGhostData.upgradedChaseSpeedMultiplier[i];

            ghosts[i].home.duration = upgradedGhostData.upgradedRespawnDuration[i];

            ghosts[i].scatter.duration = upgradedGhostData.upgradedScatterDuration[i];
            ghosts[i].scatter.scatterSpeedMultiplier = upgradedGhostData.upgradedScatterSpeedMultiplier[i];
            
            ghosts[i].frightened.duration = upgradedGhostData.upgradedFrightenedDuration[i];
            ghosts[i].frightened.frightenedSpeedMultiplier = upgradedGhostData.upgradedFrightenedSpeedMultiplier[i];
        }
    }
}

