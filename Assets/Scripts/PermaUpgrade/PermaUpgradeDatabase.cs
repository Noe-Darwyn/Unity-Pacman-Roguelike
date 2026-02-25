using UnityEngine;
using PermanentUpgradeCardSpace;
using System.Collections.Generic;
using System.Linq;

/// ScriptableObject qui contient la liste de tous les upgrades disponibles dans le jeu.
/// Permet de rechercher et filtrer les upgrades par catégorie.
[CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "Upgrades/Upgrade Database")]
public class PermaUpgradeDatabase : ScriptableObject
{
    [Header("All Available Upgrades")]
    [Tooltip("Liste complète de tous les upgrades du jeu")]
    [SerializeField] private List<PermanentUpgradeCard> allUpgrades = new List<PermanentUpgradeCard>();

    
    public List<PermanentUpgradeCard> GetAllUpgrades()
    {
        return new List<PermanentUpgradeCard>(allUpgrades); // Retourne une copie
    }

    public List<PermanentUpgradeCard> GetUpgradesByCategory(UpgradeCategory category)
    {
        return allUpgrades.Where(u => u != null && u.category == category).ToList();
    }

    public PermanentUpgradeCard GetUpgradeByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        return allUpgrades.Find(u => u != null && u.upgradeName == name);
    }

    public PermanentUpgradeCard GetUpgradeByIndex(int index)
    {
        if (index < 0 || index >= allUpgrades.Count)
        {
            Debug.LogWarning($"Index {index} out of range (database has {allUpgrades.Count} upgrades)");
            return null;
        }

        return allUpgrades[index];
    }

    public int GetUpgradeCount()
    {
        return allUpgrades.Count;
    }

    public int GetUpgradeCountByCategory(UpgradeCategory category)
    {
        return allUpgrades.Count(u => u != null && u.category == category);
    }

    /// Vérifie que la database est valide (pas de doublons, tous les upgrades configurés correctement)
    public bool ValidateDatabase()
    {
        bool isValid = true;

        // Vérifier qu'il y a au moins un upgrade
        if (allUpgrades == null || allUpgrades.Count == 0)
        {
            Debug.LogWarning("UpgradeDatabase is empty!");
            isValid = false;
        }

        // Vérifier les doublons
        HashSet<PermanentUpgradeCard> uniqueUpgrades = new HashSet<PermanentUpgradeCard>();
        foreach (PermanentUpgradeCard upgrade in allUpgrades)
        {
            if (upgrade == null)
            {
                Debug.LogWarning("UpgradeDatabase contains a null upgrade!");
                isValid = false;
                continue;
            }

            if (!uniqueUpgrades.Add(upgrade))
            {
                Debug.LogWarning($"UpgradeDatabase contains duplicate: {upgrade.upgradeName}");
                isValid = false;
            }
        }

        // Vérifier que chaque upgrade est correctement configuré
        foreach (PermanentUpgradeCard upgrade in allUpgrades)
        {
            if (upgrade == null)
                continue;

            // Vérifier que le nom n'est pas vide
            if (string.IsNullOrEmpty(upgrade.upgradeName))
            {
                Debug.LogError($"Upgrade has no name!");
                isValid = false;
            }

            // Vérifier que maxLevel est valide
            if (upgrade.maxLevel <= 0)
            {
                Debug.LogError($"{upgrade.upgradeName}: maxLevel must be > 0 (current: {upgrade.maxLevel})");
                isValid = false;
            }

            // Vérifier que l'array costs a la bonne taille
            if (upgrade.costs == null || upgrade.costs.Length != upgrade.maxLevel)
            {
                Debug.LogError($"{upgrade.upgradeName}: costs array size ({upgrade.costs?.Length ?? 0}) doesn't match maxLevel ({upgrade.maxLevel})");
                isValid = false;
            }

            // Vérifier que les coûts sont positifs
            if (upgrade.costs != null)
            {
                for (int i = 0; i < upgrade.costs.Length; i++)
                {
                    if (upgrade.costs[i] < 0)
                    {
                        Debug.LogError($"{upgrade.upgradeName}: cost at level {i + 1} is negative ({upgrade.costs[i]})");
                        isValid = false;
                    }
                }
            }
        }

        if (isValid)
        {
            Debug.Log($"UpgradeDatabase validated successfully! ({allUpgrades.Count} upgrades)");
        }
        else
        {
            Debug.LogError("UpgradeDatabase validation failed! Check errors above.");
        }

        return isValid;
    }

    
    public void AddUpgrade(PermanentUpgradeCard upgrade)
    {
        if (upgrade == null)
        {
            Debug.LogWarning("Cannot add null upgrade to database");
            return;
        }

        if (allUpgrades.Contains(upgrade))
        {
            Debug.LogWarning($"{upgrade.upgradeName} is already in the database");
            return;
        }

        allUpgrades.Add(upgrade);
        Debug.Log($"Added {upgrade.upgradeName} to database");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    
    public void RemoveUpgrade(PermanentUpgradeCard upgrade)
    {
        if (upgrade == null)
            return;

        if (allUpgrades.Remove(upgrade))
        {
            Debug.Log($"Removed {upgrade.upgradeName} from database");

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
        else
        {
            Debug.LogWarning($"{upgrade.upgradeName} was not found in database");
        }
    }

    public void SortUpgrades()
    {
        allUpgrades = allUpgrades
            .Where(u => u != null)
            .OrderBy(u => u.category)
            .ThenBy(u => u.upgradeName)
            .ToList();

        Debug.Log("Upgrades sorted by category and name");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    // ==================== DEBUG ====================

    public void DisplayDatabase()
    {
        Debug.Log($"=== UPGRADE DATABASE ===");
        Debug.Log($"Total upgrades: {allUpgrades.Count}");

        foreach (UpgradeCategory category in System.Enum.GetValues(typeof(UpgradeCategory)))
        {
            int count = GetUpgradeCountByCategory(category);
            if (count > 0)
            {
                Debug.Log($"\n{category}: {count} upgrades");

                List<PermanentUpgradeCard> categoryUpgrades = GetUpgradesByCategory(category);
                foreach (PermanentUpgradeCard upgrade in categoryUpgrades)
                {
                    if (upgrade != null)
                        Debug.Log($"  - {upgrade.upgradeName} (Max Level: {upgrade.maxLevel})");
                }
            }
        }
    }
}
