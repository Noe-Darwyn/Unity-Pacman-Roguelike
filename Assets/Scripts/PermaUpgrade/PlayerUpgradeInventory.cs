using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PermanentUpgradeCardSpace;

// ScriptableObject qui stocke l'inventaire du joueur :
/// - Currency (monnaie)
/// - Upgrades possédés avec leurs niveaux
/// - Méthodes d'achat et de vérification

[CreateAssetMenu(fileName = "PlayerInventory", menuName = "Upgrades/Player Inventory")]
public class PlayerUpgradeInventory : ScriptableObject
{
    [Header("Currency")]
    [Tooltip("Monnaie actuelle du joueur")]
    [SerializeField] private int currency = 0;

    [Header("Owned Upgrades")]
    [Tooltip("Liste des upgrades possédés par le joueur avec leurs niveaux")]
    [SerializeField] private List<UpgradeData> ownedUpgrades = new List<UpgradeData>();

    /// Classe interne pour sérialiser les upgrades possédés
    [System.Serializable]
    public class UpgradeData
    {
        public PermanentUpgradeCard card;
        public int currentLevel;

        public UpgradeData(PermanentUpgradeCard card, int level)
        {
            this.card = card;
            this.currentLevel = level;
        }
    }

    /// Obtient ou définit la monnaie du joueur
    public int Currency
    {
        get { return currency; }
        set { currency = Mathf.Max(0, value); } // Ne peut pas être négatif
    }


    public int GetCurrentLevel(PermanentUpgradeCard card)
    {
        if (card == null)
            return 0;

        UpgradeData data = ownedUpgrades.Find(u => u.card == card);
        return data != null ? data.currentLevel : 0;
    }

    public bool OwnsUpgrade(PermanentUpgradeCard card)
    {
        return GetCurrentLevel(card) > 0;
    }

    public bool IsMaxLevel(PermanentUpgradeCard card)
    {
        if (card == null)
            return false;

        int currentLevel = GetCurrentLevel(card);
        return currentLevel >= card.maxLevel;
    }

    public List<UpgradeData> GetAllOwnedUpgrades()
    {
        return new List<UpgradeData>(ownedUpgrades); // Retourne une copie
    }

    public bool CanPurchase(PermanentUpgradeCard card, int targetLevel)
    {
        if (card == null)
        {
            Debug.LogError("Cannot purchase: card is null");
            return false;
        }

        int currentLevel = GetCurrentLevel(card);

        // Vérifications
        if (targetLevel != currentLevel + 1)
        {
            Debug.LogWarning($"Cannot purchase {card.upgradeName}: target level {targetLevel} must be current level + 1 (current: {currentLevel})");
            return false;
        }

        if (targetLevel > card.maxLevel)
        {
            Debug.LogWarning($"Cannot purchase {card.upgradeName}: target level {targetLevel} exceeds max level {card.maxLevel}");
            return false;
        }

        // Vérifier le coût
        if (card.costs == null || card.costs.Length < targetLevel)
        {
            Debug.LogError($"Cannot purchase {card.upgradeName}: costs array is invalid (length: {card.costs?.Length ?? 0})");
            return false;
        }

        int cost = card.costs[targetLevel - 1]; // costs[0] = level 1
        if (currency < cost)
        {
            Debug.Log($"Cannot purchase {card.upgradeName} level {targetLevel}: insufficient funds ({currency}/{cost})");
            return false;
        }

        return true;
    }

    public bool PurchaseUpgrade(PermanentUpgradeCard card, int targetLevel)
    {
        // Vérifier si l'achat est possible
        if (!CanPurchase(card, targetLevel))
            return false;

        int cost = card.costs[targetLevel - 1];

        // Déduire le coût
        currency -= cost;

        // Mettre à jour ou ajouter l'upgrade
        UpgradeData existing = ownedUpgrades.Find(u => u.card == card);

        if (existing != null)
        {
            existing.currentLevel = targetLevel;
        }
        else
        {
            ownedUpgrades.Add(new UpgradeData(card, targetLevel));
        }

        Debug.Log($"Purchased {card.upgradeName} level {targetLevel} for {cost} currency. Remaining: {currency}");
        return true;
    }

    public void AddCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative currency. Use RemoveCurrency instead.");
            return;
        }

        currency += amount;
        Debug.Log($"Added {amount} currency. Total: {currency}");
    }

    public void RemoveCurrency(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot remove negative currency. Use AddCurrency instead.");
            return;
        }

        currency = Mathf.Max(0, currency - amount);
        Debug.Log($"Removed {amount} currency. Total: {currency}");
    }

    // ==================== RESET ET DEBUG ====================

    public void ResetInventory()
    {
        currency = 0;
        ownedUpgrades.Clear();
        Debug.LogWarning("Inventory reset.");
    }

    public void SetCurrency(int amount)
    {
        currency = Mathf.Max(0, amount);
        Debug.LogWarning($"Currency set to {currency}");
    }

    public void AddUpgradeDirectly(PermanentUpgradeCard card, int level)
    {
        if (card == null)
        {
            Debug.LogError("Cannot add null upgrade");
            return;
        }

        level = Mathf.Clamp(level, 1, card.maxLevel);

        UpgradeData existing = ownedUpgrades.Find(u => u.card == card);

        if (existing != null)
        {
            existing.currentLevel = level;
        }
        else
        {
            ownedUpgrades.Add(new UpgradeData(card, level));
        }

        Debug.LogWarning($"Added {card.upgradeName} at level {level} (bypassed purchase)");
    }

    public void DisplayInventory()
    {
        Debug.Log($"=== PLAYER INVENTORY ===");
        Debug.Log($"Currency: {currency}");
        Debug.Log($"Owned Upgrades: {ownedUpgrades.Count}");

        foreach (UpgradeData data in ownedUpgrades)
        {
            if (data.card != null)
                Debug.LogWarning($"  - {data.card.upgradeName}: Level {data.currentLevel}/{data.card.maxLevel}");
        }
    }

    // ======= ET ON DIT TOUS MERCI CLAUDE.AI =======

    // ==================== PERSISTENCE (À IMPLÉMENTER PLUS TARD) ====================
    // Ces méthodes seront implémentées quand on ajoutera la sauvegarde JSON

    /// <summary>
    /// Sauvegarde l'inventaire dans un fichier JSON
    /// TODO: Implémenter plus tard
    /// </summary>
    public void SaveToFile()
    {
        Debug.LogWarning("SaveToFile not implemented yet");
        // TODO: Sérialiser en JSON et sauvegarder
    }

    /// <summary>
    /// Charge l'inventaire depuis un fichier JSON
    /// TODO: Implémenter plus tard
    /// </summary>
    public void LoadFromFile()
    {
        Debug.LogWarning("LoadFromFile not implemented yet");
        // TODO: Charger depuis JSON et désérialiser
    }
}