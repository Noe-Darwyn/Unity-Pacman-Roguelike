using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GhostCardSpace
{
    [CreateAssetMenu(fileName = "New Ghost Card", menuName = "Ghost Card")]
    public class GhostCard : ScriptableObject
    {
        // Basic Info
        [Header("Basic Info")]
        public Sprite Sprite;
        public string ghostName;
        public int Health;
        public int Points = 0;

        // Traits and Modifiers
        [Header("Traits and Modifiers")]
        public GhostTrait trait;

        public enum GhostTrait
        {
            Imprevisible,
            Agressive,
            Ambusher,
            Duplicitous
        }

        public GhostModifier modifier;
        public enum GhostModifier
        {
            None,
            Fast,
            Slow,
            Tanky,
            Fragile
        }
        
        // Movement and Behavior Stats
        [Header("Movement and Behavior Stats")]
        [Header("Base Stats")]
        public int baseSpeed;
        public int baseSpeedMultiplier;
        [Header("Chase Stats")]
        public int chaseDuration;
        public int chaseSpeedMutliplier;
        public int packProximity;
        [Header("Spawn Stats")]  
        public int respawnDuration;
        public DeploymentCondition deploymentCondition;
        public enum DeploymentCondition
        {
            TimeBased,
            ScoreBased,
            EventBased
        }
        [Header("Scatter Stats")]
        public int scatterDuration;
        public int scatterSpeedMultiplier;
        public int scatterProximity;
        public ScatterCorner scatterCorner;
        public enum ScatterCorner
        {
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight
        }
        [Header("Frightened Stats")]
        public int frightenedSpeedMultiplier;
        
        public void DisplayCardInfo()
        {
            Debug.Log($"Ghost Name: {ghostName}\n" +
                      $"Health: {Health}\n" +
                      $"Points: {Points}\n" +
                      $"Trait: {trait}\n" +
                      $"Modifier: {modifier}\n" +
                      $"Base Speed: {baseSpeed}\n" +
                      $"Base Speed Multiplier: {baseSpeedMultiplier}\n" +
                      $"Chase Duration: {chaseDuration}\n" +
                      $"Chase Speed Multiplier: {chaseSpeedMutliplier}\n" +
                      $"Pack Proximity: {packProximity}\n" +
                      $"Respawn Duration: {respawnDuration}\n" +
                      $"Deployment Condition: {deploymentCondition}\n" +
                      $"Scatter Duration: {scatterDuration}\n" +
                      $"Scatter Speed Multiplier: {scatterSpeedMultiplier}\n" +
                      $"Scatter Proximity: {scatterProximity}\n" +
                      $"Scatter Corner: {scatterCorner}\n" +
                      $"Frightened Speed Multiplier: {frightenedSpeedMultiplier}");
        }
    }
}
