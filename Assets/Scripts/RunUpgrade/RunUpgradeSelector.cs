using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TemporaryUpgradeCardSpace;

public class RunUpgradeSelector : MonoBehaviour
{
    [SerializeField] private RunUpgradeDatabase database;
    [SerializeField] private RunInventory inventory;

    public List<TemporaryUpgradeCard> GetRandomChoices(int count)
    {
        var pool = database.allUpgrades
            .Where(card => !inventory.IsMaxLevel(card)) // éviter upgrades maxées
            .ToList();

        if (pool.Count == 0)
        {
            Debug.LogWarning("No upgrades available!");
            return new List<TemporaryUpgradeCard>();
        }

        // Mélange simple
        return pool
            .OrderBy(_ => Random.value)
            .Take(count)
            .ToList();
    }
}
