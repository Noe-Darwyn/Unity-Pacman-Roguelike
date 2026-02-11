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

    public void BuildGhosts(Ghost ghostPrefab, Transform ghostParent, Pacman pacman, Transform ghostHomeInside, Transform ghostHomeOutside)
    {
        CreateGhosts(ghostPrefab, ghostParent, pacman, ghostHomeInside, ghostHomeOutside);
        
        SetGhostStats();
    }

    void CreateGhosts(Ghost ghostPrefab, Transform ghostParent, Pacman pacman, Transform ghostHomeInside, Transform ghostHomeOutside)
    {
        if (ghostPrefab == null)
        {
            Debug.LogError("GhostBuilder: Ghost prefab is not assigned!");
            return;
        }

        GhostCard[] ghostCardData = upgradedGhostData.GetGhostCardData();
        ghosts = new Ghost[ghostCardData.Length];

        for (int i = 0; i < ghostCardData.Length; i++)
        {
            // Instancier le fantôme
            Ghost ghostInstance = Instantiate(ghostPrefab, ghostParent);
            ghosts[i] = ghostInstance;

            // Assigner Pacman comme target
            ghostInstance.target = pacman.transform;

            // Assigner les positions inside/outside pour GhostHome
            GhostHome ghostHome = ghostInstance.GetComponent<GhostHome>();
            if (ghostHome != null)
            {
                ghostHome.inside = ghostHomeInside;
                ghostHome.outside = ghostHomeOutside;
            }
        }
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

