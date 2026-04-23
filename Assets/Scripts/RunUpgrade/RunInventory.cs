using System.Collections.Generic;
using UnityEngine;
using TemporaryUpgradeCardSpace;

public class RunInventory : MonoBehaviour
{
    public List<RunUpgradeInstance> upgrades = new List<RunUpgradeInstance>();
    
    
    /**
    ==== TEST ====


    [SerializeField] private TemporaryUpgradeCard testCard;

    void Start()
    {
        Debug.Log("RUNINVENTORY START");
        if (testCard != null)
        {
            AddOrUpgrade(testCard);
            AddOrUpgrade(testCard);
            DisplayInventory();
        }
    }

    */

    public void AddOrUpgrade(TemporaryUpgradeCard card)
    {
        Debug.Log("RUNINVENTORY ADD OR UPGRADE");
        if (card == null)
        {
            Debug.LogError("RunInventory: card is null!");
            return;
        }

        RunUpgradeInstance existing = upgrades.Find(u => u.card == card);

        if (existing != null)
        {
            existing.LevelUp();
            Debug.Log($"Upgrade {card.temporaryUpgradeName} leveled up to {existing.level}");
        }
        else
        {
            upgrades.Add(new RunUpgradeInstance(card));
            Debug.Log($"Added new upgrade {card.temporaryUpgradeName} (level 1)");
        }
    }

    //GETTERS
 
    public List<RunUpgradeInstance> GetAllUpgrades()
    {
        return upgrades;
    }

    public RunUpgradeInstance GetUpgrade(TemporaryUpgradeCard card)
    {
        return upgrades.Find(u => u.card == card);
    }

    public int GetLevel(TemporaryUpgradeCard card)
    {
        var upgrade = GetUpgrade(card);
        return upgrade != null ? upgrade.level : 0;
    }

    public bool HasUpgrade(TemporaryUpgradeCard card)
    {
        return GetLevel(card) > 0;
    }

    public bool IsMaxLevel(TemporaryUpgradeCard card)
    {
        var upgrade = GetUpgrade(card);
        return upgrade != null && upgrade.IsMaxLevel();
    }

    // RESET (nouvelle run)
    public void ResetRun()
    {
        upgrades.Clear();
        Debug.Log("RunInventory reset.");
    }

    // DEBUG
    public void DisplayInventory()
    {
        Debug.Log("=== RUNINVENTORY DISPLAY ===");

        foreach (var u in upgrades)
        {
            Debug.Log($"{u.card.temporaryUpgradeName} - Level {u.level}/{u.card.maxLevel}");
        }
    }
}
