using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PermanentUpgradeCardSpace
{
    [CreateAssetMenu(fileName = "New Permanent Upgrade Card", menuName = "Permanent Upgrade Card")] 
    public class PermanentUpgradeCard : ScriptableObject
    {
        [Header("== Upgrade system ==")]
        [Tooltip("Niveau maximum de cet upgrade (1 = upgrade unique)")]
        public int maxLevel = 1;

        [Tooltip("Catégorie de l'upgrade")]
        public UpgradeCategory category;

        [Tooltip("Couts d'achat pour chaque niveau. Taille du tableau doit être égale à maxLevel")]
        public int[] costs;


        [Header("Basic Info")]
        public Sprite upgradeSprite;
        public string upgradeName;
        public string upgradeDescription;

        [Header("Health and Points Upgrades")]
        public int[] healthIncrease;
        public int[] pointsDecrease;
        
        [Header("Movement and Behavior Upgrades")]
        [Header("- Basic Stats")]
        public float[] baseSpeedIncrease;
        public float[] baseSpeedMultiplierIncrease;

        [Header("- Chase Stats")]
        public int[] chaseDurationIncrease;
        public float[] chaseSpeedMultiplierIncrease;
        public int[] packProximityIncrease;

        [Header("- Spawn Stats")]  
        public int[] respawnDurationDecrease;

        [Header("- Scatter Stats")]
        public int[] scatterDurationIncrease;
        public float[] scatterSpeedMultiplierIncrease;
        public int[] cornerProximityIncrease;

        [Header("- Frightened Stats")]
        public int[] frightenedDurationDecrease;
        public float[] frightenedSpeedMultiplierIncrease;

        public void DisplayCardInfo()
        {
            Debug.Log($"Health Increase: {healthIncrease}\n" +
                      $"Points Decrease: {pointsDecrease}\n" +
                      $"Base Speed Increase: {baseSpeedIncrease}\n" +
                      $"Base Speed Multiplier Increase: {baseSpeedMultiplierIncrease}\n" +
                      $"Chase Duration Increase: {chaseDurationIncrease}\n" +
                      $"Chase Speed Multiplier Increase: {chaseSpeedMultiplierIncrease}\n" +
                      $"Pack Proximity Increase: {packProximityIncrease}\n" +
                      $"Respawn Duration Decrease: {respawnDurationDecrease}\n" +
                      $"Scatter Duration Increase: {scatterDurationIncrease}\n" +
                      $"Scatter Speed Multiplier Increase: {scatterSpeedMultiplierIncrease}\n" +
                      $"Corner Proximity Increase: {cornerProximityIncrease}\n" +
                      $"Frightened Duration Decrease: {frightenedDurationDecrease}\n" +
                      $"Frightened Speed Multiplier Increase: {frightenedSpeedMultiplierIncrease}");
        }

        public bool ValidateArraySizes()
        {
            bool isValid = true;
            if (costs == null || costs.Length != maxLevel)
            {
                Debug.LogError("${upgradeName}: costs array size ({costs?.Length ?? 0}) does not match maxLevel ({maxLevel})");
                isValid = false;
            }
            return isValid;
        }
    }
}
