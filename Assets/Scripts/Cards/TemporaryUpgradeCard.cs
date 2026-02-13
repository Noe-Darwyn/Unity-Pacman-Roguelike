using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TemporaryUpgradeCardSpace
{
    [CreateAssetMenu(fileName = "New Temporary Upgrade Card", menuName = "Temporary Upgrade Card")]
    public class TemporaryUpgradeCard : ScriptableObject
    {
        [Header("Basic Info")]
        public Sprite temporaryUpgradeSprite;
        public string temporaryUpgradeName;
        public string temporaryUpgradeDescription;

        [Header("Movement and Behavior Upgrades")]
        [Header("Chase Stats")]
        public int chaseDurationAlteration;
        public float chaseSpeedMultiplierAlteration;
        public int packProximityAlteration;

        [Header("Scatter Stats")]
        public int scatterDurationAlteration;
        public float scatterSpeedMultiplierAlteration;
        public int cornerProximityAlteration;

        public void DisplayTemporaryUpgradeCardInfo()
        {
            Debug.Log($"Temporary Upgrade Name: {temporaryUpgradeName}\n" +
                      $"Temporary Upgrade Description: {temporaryUpgradeDescription}\n" +
                      $"Chase Duration Alteration: {chaseDurationAlteration}\n" +
                      $"Chase Speed Multiplier Alteration: {chaseSpeedMultiplierAlteration}\n" +
                      $"Pack Proximity Alteration: {packProximityAlteration}\n" +
                      $"Scatter Duration Alteration: {scatterDurationAlteration}\n" +
                      $"Scatter Speed Multiplier Alteration: {scatterSpeedMultiplierAlteration}\n" +
                      $"Corner Proximity Alteration: {cornerProximityAlteration}");
        }
    }
}
