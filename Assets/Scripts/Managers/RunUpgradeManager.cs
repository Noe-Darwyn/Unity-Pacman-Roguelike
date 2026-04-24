using UnityEngine;
using System.Collections.Generic;
using TemporaryUpgradeCardSpace;

public class RunUpgradeManager : MonoBehaviour
{
    [SerializeField] private RunInventory inventory;

    // =========================
    // CHASE
    // =========================
    public int totalChaseDuration { get; private set; }
    public float totalChaseSpeedMultiplier { get; private set; }
    public int totalPackProximity { get; private set; }

    // =========================
    // SCATTER
    // =========================
    public int totalScatterDuration { get; private set; }
    public float totalScatterSpeedMultiplier { get; private set; }
    public int totalCornerProximity { get; private set; }

    public void RecalculateStats()
    {
        ResetStats();

        foreach (var upgrade in inventory.GetAllUpgrades())
        {
            if (upgrade == null || upgrade.card == null)
                continue;

            Debug.Log($"[RUN UPGRADE] {upgrade.card.temporaryUpgradeName} level={upgrade.level}");

            if (upgrade.level <= 0)
            {
                Debug.LogError($"INVALID LEVEL (<=0) for {upgrade.card.temporaryUpgradeName}");
                continue;
            }

            if (upgrade.level > upgrade.card.maxLevel)
            {
                Debug.LogError($"LEVEL > MAX for {upgrade.card.temporaryUpgradeName}");
                continue;
            }

            if (!upgrade.card.ValidateArraySizes())
            {
                Debug.LogError($"ARRAY SIZE INVALID for {upgrade.card.temporaryUpgradeName}");
                continue;
            }

            var card = upgrade.card;
            int level = upgrade.level;

            // CHASE
            totalChaseDuration += card.GetChaseDuration(level);
            totalChaseSpeedMultiplier += card.GetChaseSpeedMultiplier(level);
            totalPackProximity += card.GetPackProximity(level);

            // SCATTER
            totalScatterDuration += card.GetScatterDuration(level);
            totalScatterSpeedMultiplier += card.GetScatterSpeedMultiplier(level);
            totalCornerProximity += card.GetCornerProximity(level);
        }
    }

    private void ResetStats()
    {
        totalChaseDuration = 0;
        totalChaseSpeedMultiplier = 0f;
        totalPackProximity = 0;

        totalScatterDuration = 0;
        totalScatterSpeedMultiplier = 0f;
        totalCornerProximity = 0;
    }
}