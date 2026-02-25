using UnityEngine;
using PermanentUpgradeCardSpace;
[System.Serializable]
public class PermaUpgradeInstance
{
    [Tooltip("Référence vers la carte d'upgrade (ScriptableObject)")]
    public PermanentUpgradeCard card;
    
    [Tooltip("Niveau actuel de cet upgrade (1 à card.maxLevel)")]
    public int currentLevel;

    // Constructeur
    public PermaUpgradeInstance(PermanentUpgradeCard card, int level)
    {
        this.card = card;
        this.currentLevel = Mathf.Clamp(level, 0, card != null ? card.maxLevel : 0);
    }

    // ==================== GETTERS POUR LES STATS ====================
    // Chaque getter retourne la valeur de la stat au niveau actuel
    // Si l'array est null/vide ou si currentLevel = 0, retourne 0
    
    public int GetHealthIncrease()
    {
        if (card == null || card.healthIncrease == null || currentLevel == 0 || currentLevel > card.healthIncrease.Length)
            return 0;
        
        return card.healthIncrease[currentLevel - 1]; // -1 car array commence à 0
    }

    public int GetPointsDecrease()
    {
        if (card == null || card.pointsDecrease == null || currentLevel == 0 || currentLevel > card.pointsDecrease.Length)
            return 0;
        
        return card.pointsDecrease[currentLevel - 1];
    }

    public float GetBaseSpeedIncrease()
    {
        if (card == null || card.baseSpeedIncrease == null || currentLevel == 0 || currentLevel > card.baseSpeedIncrease.Length)
            return 0f;
        
        return card.baseSpeedIncrease[currentLevel - 1];
    }

    public float GetBaseSpeedMultiplierIncrease()
    {
        if (card == null || card.baseSpeedMultiplierIncrease == null || currentLevel == 0 || currentLevel > card.baseSpeedMultiplierIncrease.Length)
            return 0f;
        
        return card.baseSpeedMultiplierIncrease[currentLevel - 1];
    }

    public int GetChaseDurationIncrease()
    {
        if (card == null || card.chaseDurationIncrease == null || currentLevel == 0 || currentLevel > card.chaseDurationIncrease.Length)
            return 0;
        
        return card.chaseDurationIncrease[currentLevel - 1];
    }

    public float GetChaseSpeedMultiplierIncrease()
    {
        if (card == null || card.chaseSpeedMultiplierIncrease == null || currentLevel == 0 || currentLevel > card.chaseSpeedMultiplierIncrease.Length)
            return 0f;
        
        return card.chaseSpeedMultiplierIncrease[currentLevel - 1];
    }

    public int GetPackProximityIncrease()
    {
        if (card == null || card.packProximityIncrease == null || currentLevel == 0 || currentLevel > card.packProximityIncrease.Length)
            return 0;
        
        return card.packProximityIncrease[currentLevel - 1];
    }

    public int GetRespawnDurationDecrease()
    {
        if (card == null || card.respawnDurationDecrease == null || currentLevel == 0 || currentLevel > card.respawnDurationDecrease.Length)
            return 0;
        
        return card.respawnDurationDecrease[currentLevel - 1];
    }

    public int GetScatterDurationIncrease()
    {
        if (card == null || card.scatterDurationIncrease == null || currentLevel == 0 || currentLevel > card.scatterDurationIncrease.Length)
            return 0;
        
        return card.scatterDurationIncrease[currentLevel - 1];
    }

    public float GetScatterSpeedMultiplierIncrease()
    {
        if (card == null || card.scatterSpeedMultiplierIncrease == null || currentLevel == 0 || currentLevel > card.scatterSpeedMultiplierIncrease.Length)
            return 0f;
        
        return card.scatterSpeedMultiplierIncrease[currentLevel - 1];
    }

    public int GetCornerProximityIncrease()
    {
        if (card == null || card.cornerProximityIncrease == null || currentLevel == 0 || currentLevel > card.cornerProximityIncrease.Length)
            return 0;
        
        return card.cornerProximityIncrease[currentLevel - 1];
    }

    public int GetFrightenedDurationDecrease()
    {
        if (card == null || card.frightenedDurationDecrease == null || currentLevel == 0 || currentLevel > card.frightenedDurationDecrease.Length)
            return 0;
        
        return card.frightenedDurationDecrease[currentLevel - 1];
    }

    public float GetFrightenedSpeedMultiplierIncrease()
    {
        if (card == null || card.frightenedSpeedMultiplierIncrease == null || currentLevel == 0 || currentLevel > card.frightenedSpeedMultiplierIncrease.Length)
            return 0f;
        
        return card.frightenedSpeedMultiplierIncrease[currentLevel - 1];
    }

    public int GetNextLevelCost()
    {
        if (card == null || card.costs == null || currentLevel >= card.maxLevel)
            return 0; // Déjà au max
        
        if (currentLevel >= card.costs.Length)
            return 0; // Array mal configuré
        
        return card.costs[currentLevel]; // currentLevel car costs[0] = coût du level 1
    }

    public bool IsMaxLevel()
    {
        return card != null && currentLevel >= card.maxLevel;
    }

    public override string ToString()
    {
        if (card == null)
            return "PermaUpgradeInstance (no card)";
        
        return $"{card.upgradeName} (Level {currentLevel}/{card.maxLevel})";
    }
}
