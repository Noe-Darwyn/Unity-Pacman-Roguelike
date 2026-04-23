using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TemporaryUpgradeCardSpace
{
    [CreateAssetMenu(fileName = "New Temporary Upgrade Card", menuName = "Temporary Upgrade Card")]
    public class TemporaryUpgradeCard : ScriptableObject
    {
        [Header("==Upgrade System ==")]
        [Tooltip("Niveau maximum de cette upgrade (1 = upgrade unique)")]
        public int maxLevel = 1;

        [Header("Basic Info")]
        public Sprite temporaryUpgradeSprite;
        public string temporaryUpgradeName;
        public string temporaryUpgradeDescription;

        //[Header("Movement and Behavior Upgrades")]
        [Header("Chase Stats")]
        public int[] chaseDurationAlteration;
        public float[] chaseSpeedMultiplierAlteration;
        public int[] packProximityAlteration;

        [Header("Scatter Stats")]
        public int[] scatterDurationAlteration;
        public float[] scatterSpeedMultiplierAlteration;
        public int[] cornerProximityAlteration;

        public bool ValidateArraySizes()
        {
            bool isValid = true;

            if (!CheckArraySize(chaseDurationAlteration, nameof(chaseDurationAlteration))) isValid = false;
            if (!CheckArraySize(chaseSpeedMultiplierAlteration, nameof(chaseSpeedMultiplierAlteration))) isValid = false;
            if (!CheckArraySize(packProximityAlteration, nameof(packProximityAlteration))) isValid = false;

            if (!CheckArraySize(scatterDurationAlteration, nameof(scatterDurationAlteration))) isValid = false;
            if (!CheckArraySize(scatterSpeedMultiplierAlteration, nameof(scatterSpeedMultiplierAlteration))) isValid = false;
            if (!CheckArraySize(cornerProximityAlteration, nameof(cornerProximityAlteration))) isValid = false;

            return isValid;
        }

        public bool CheckArraySize(System.Array array, string fieldName)
        {
            if (array == null || array.Length != maxLevel)
            {
                Debug.LogError($"{temporaryUpgradeName}: {fieldName} size ({array?.Length ?? 0}) does not match maxLevel ({maxLevel})");
                return false;
            }
            return true;
        }

        //GETTERS 

        public int GetChaseDuration(int level){
            return chaseDurationAlteration[level-1];
        }

        public float GetChaseSpeedMultiplier(int level){
            return chaseSpeedMultiplierAlteration[level-1];
        }

        public int GetPackProximity(int level){
            return packProximityAlteration[level-1];
        }

        public int GetScatterDuration(int level){
            return scatterDurationAlteration[level-1];
        }

        public float GetScatterSpeedMultiplier(int level){
            return scatterSpeedMultiplierAlteration[level-1];
        }

        public int GetCornerProximity(int level){
            return cornerProximityAlteration[level-1];
        }


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
