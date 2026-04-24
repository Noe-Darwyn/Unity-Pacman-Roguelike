using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostCardSpace;

public class GhostBuilder : MonoBehaviour
{
    [SerializeField] private PermanentUpgradeManager upgradedGhostData;
    [SerializeField] private RunUpgradeManager runUpgradeManager;
    [Header("Ghost Spawning")]
    [Space(10)]
    [SerializeField] private Ghost ghostPrefab;
    [SerializeField] private Transform ghostParent;
    [SerializeField] private Transform ghostHomeInside;
    [SerializeField] private Transform ghostHomeOutside;

    public Ghost[] ghosts;

    void Awake()
    {
        if (upgradedGhostData == null)
        {
            Debug.LogError("[GhostBuilder] Missing reference: upgradedGhostData is not assigned.");
            return;
        }
    } 

    public void BuildGhosts(Pacman pacman)
    {
        if (!ValidateReferences(pacman))
        {
            ghosts = new Ghost[0];
            return;
        }

        CreateGhosts(ghostPrefab, ghostParent, pacman, ghostHomeInside, ghostHomeOutside);

        if (ghosts == null || ghosts.Length == 0)
        {
            Debug.LogError("[GhostBuilder] Build failed: no ghosts were created.");
            return;
        }
        
        SetGhostStats();
    }

    private bool ValidateReferences(Pacman pacman)
    {
        if (upgradedGhostData == null)
        {
            Debug.LogError("[GhostBuilder] Missing reference: upgradedGhostData is not assigned.");
            return false;
        }

        if (ghostPrefab == null)
        {
            Debug.LogError("[GhostBuilder] Missing reference: ghostPrefab is not assigned.");
            return false;
        }

        if (ghostParent == null)
        {
            Debug.LogError("[GhostBuilder] Missing reference: ghostParent is not assigned.");
            return false;
        }

        if (ghostHomeInside == null || ghostHomeOutside == null)
        {
            Debug.LogError("[GhostBuilder] Missing reference: ghostHomeInside/ghostHomeOutside is not assigned.");
            return false;
        }

        if (pacman == null)
        {
            Debug.LogError("[GhostBuilder] Missing reference: pacman is not assigned.");
            return false;
        }

        return true;
    }

    void CreateGhosts(Ghost ghostPrefab, Transform ghostParent, Pacman pacman, Transform ghostHomeInside, Transform ghostHomeOutside)
    {
        GhostCard[] ghostCardData = upgradedGhostData.GetGhostCardData();
        if (ghostCardData == null || ghostCardData.Length == 0)
        {
            Debug.LogWarning("[GhostBuilder] No ghost card data found. Nothing to spawn.");
            ghosts = new Ghost[0];
            return;
        }

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

/*
    void SetGhostStats()
    {
        if (ghosts == null || ghosts.Length == 0)
        {
            Debug.LogWarning("[GhostBuilder] SetGhostStats skipped: no ghosts to configure.");
            return;
        }

        GhostCard[] ghostCardData = upgradedGhostData.GetGhostCardData();
        if (ghostCardData == null || ghostCardData.Length == 0)
        {
            Debug.LogWarning("[GhostBuilder] SetGhostStats skipped: no ghost card data found.");
            return;
        }

        int count = Mathf.Min(ghosts.Length, ghostCardData.Length);
        if (count != ghosts.Length || count != ghostCardData.Length)
        {
            Debug.LogWarning("[GhostBuilder] Data size mismatch between spawned ghosts and ghost card data. Applying stats to the matching subset.");
        }
        
        for (int i = 0; i < count; i++)
        {
            ghosts[i].initialBehaviorType = ghostCardData[i].initialBehaviorType;

            // Initialiser le GhostLifeManager avec les vies améliorées
            ghosts[i].lifeManager.Initialize(upgradedGhostData.upgradedLives[i]);
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
    */
    void SetGhostStats()
    {
        if (ghosts == null || ghosts.Length == 0)
            return;

        GhostCard[] ghostCardData = upgradedGhostData.GetGhostCardData();
        int count = Mathf.Min(ghosts.Length, ghostCardData.Length);

        for (int i = 0; i < count; i++)
        {
            // =========================
            // BASE + PERMANENT
            // =========================
            int lives = upgradedGhostData.upgradedLives[i];
            int points = upgradedGhostData.upgradedPoints[i];

            float baseSpeed = upgradedGhostData.upgradedBaseSpeed[i];
            float baseSpeedMultiplier = upgradedGhostData.upgradedBaseSpeedMultiplier[i];

            int chaseDuration = upgradedGhostData.upgradedChaseDuration[i];
            float chaseSpeedMultiplier = upgradedGhostData.upgradedChaseSpeedMultiplier[i];

            int respawnDuration = upgradedGhostData.upgradedRespawnDuration[i];

            int scatterDuration = upgradedGhostData.upgradedScatterDuration[i];
            float scatterSpeedMultiplier = upgradedGhostData.upgradedScatterSpeedMultiplier[i];

            int frightenedDuration = upgradedGhostData.upgradedFrightenedDuration[i];
            float frightenedSpeedMultiplier = upgradedGhostData.upgradedFrightenedSpeedMultiplier[i];

            // 👇 SI TES GHOSTS SUPPORTENT CES STATS
            int packProximity = ghostCardData[i].packProximity;
            int cornerProximity = ghostCardData[i].cornerProximity;

            // =========================
            // RUN UPGRADES
            // =========================
            if (runUpgradeManager != null)
            {
                chaseDuration += runUpgradeManager.totalChaseDuration;
                chaseSpeedMultiplier += runUpgradeManager.totalChaseSpeedMultiplier;
                packProximity += runUpgradeManager.totalPackProximity;

                scatterDuration += runUpgradeManager.totalScatterDuration;
                scatterSpeedMultiplier += runUpgradeManager.totalScatterSpeedMultiplier;
                cornerProximity += runUpgradeManager.totalCornerProximity;
            }

            // =========================
            // APPLY
            // =========================
            ghosts[i].lifeManager.Initialize(lives);
            ghosts[i].points = points;

            ghosts[i].movement.speed = baseSpeed;
            ghosts[i].movement.speedMultiplier = baseSpeedMultiplier;

            ghosts[i].chase.duration = chaseDuration;
            ghosts[i].chase.chaseSpeedMultiplier = chaseSpeedMultiplier;

            ghosts[i].home.duration = respawnDuration;

            ghosts[i].scatter.duration = scatterDuration;
            ghosts[i].scatter.scatterSpeedMultiplier = scatterSpeedMultiplier;

            ghosts[i].frightened.duration = frightenedDuration;
            ghosts[i].frightened.frightenedSpeedMultiplier = frightenedSpeedMultiplier;

            // IMPORTANT : seulement si ces champs existent

            //ghosts[i].chase.packProximity = packProximity;
            //ghosts[i].scatter.cornerProximity = cornerProximity;

            Debug.Log($"[GhostBuilder] Applied stats to {ghosts[i].ghostName}: lives={lives}, points={points}, baseSpeed={baseSpeed}, baseSpeedMultiplier={baseSpeedMultiplier}, chaseDuration={chaseDuration}, chaseSpeedMultiplier={chaseSpeedMultiplier}, respawnDuration={respawnDuration}, scatterDuration={scatterDuration}, scatterSpeedMultiplier={scatterSpeedMultiplier}, frightenedDuration={frightenedDuration}, frightenedSpeedMultiplier={frightenedSpeedMultiplier}, packProximity={packProximity}, cornerProximity={cornerProximity}");
        }
    }

    public void ReapplyStats()
    {
        SetGhostStats();
    }
}

