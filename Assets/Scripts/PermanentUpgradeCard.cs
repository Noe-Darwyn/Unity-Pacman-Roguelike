using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PermanentUpgradeCardSpace
{
    [CreateAssetMenu(fileName = "New Permanent Upgrade Card", menuName = "Permanent Upgrade Card")] 
    public class PermanentUpgradeCard : ScriptableObject
    {
        [Header("Basic Info")]
        public Sprite upgradeSprite;
        public string upgradeName;
        public string upgradeDescription;

        [Header("Health and Points Upgrades")]
        public int healthIncrease;
        public int pointsDecrease;
        
        [Header("Movement and Behavior Upgrades")]
        [Header("- Basic Stats")]
        public int baseSpeedIncrease;
        public int baseSpeedMultiplierIncrease;

        [Header("- Chase Stats")]
        public int chaseDurationIncrease;
        public int chaseSpeedMultiplierIncrease;
        public int packProximityIncrease;

        [Header("- Spawn Stats")]  
        public int respawnDurationDecrease;

        [Header("- Scatter Stats")]
        public int scatterDurationIncrease;
        public int scatterSpeedMultiplierIncrease;
        public int cornerProximityIncrease;

        [Header("- Frightened Stats")]
        public int frightenedDurationDecrease;
        public int frightenedSpeedMultiplierIncrease;

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
    }
}
