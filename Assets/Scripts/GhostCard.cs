using Unity.Mathematics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GhostCardSpace
{
    [CreateAssetMenu(fileName = "New Ghost Card", menuName = "Ghost Card")]
    public class GhostCard : ScriptableObject
    {
        [Header("Basic Info")]
        public Sprite Sprite;
        public string ghostName;
        public string description;

        [Header("Health and Points")]
        public int health;
        public int points = 0;

        [Header("Movement and Behavior Stats")]
        public GhostBehaviorType initialBehaviorType;
        
        [Header("- Base Stats")]
        public int baseSpeed;
        public int baseSpeedMultiplier;

        [Header("- Chase Stats")]
        public int chaseDuration;
        public int chaseSpeedMultiplier;
        public int packProximity;

        [Header("- Spawn Stats")]  
        public int respawnDuration;

        [Header("- Scatter Stats")]
        public int scatterDuration;
        public int scatterSpeedMultiplier;
        public int cornerProximity;

        [Header("- Frightened Stats")]
        public int frightenedDuration;
        public int frightenedSpeedMultiplier;
        
        public void DisplayCardInfo()
        {
            Debug.Log($"Health: {health}\n" +
                      $"Points: {points}\n" +
                      $"Base Speed: {baseSpeed}\n" +
                      $"Base Speed Multiplier: {baseSpeedMultiplier}\n" +
                      $"Chase Duration: {chaseDuration}\n" +
                      $"Chase Speed Multiplier: {chaseSpeedMultiplier}\n" +
                      $"Pack Proximity: {packProximity}\n" +
                      $"Respawn Duration: {respawnDuration}\n" +
                      $"Scatter Duration: {scatterDuration}\n" +
                      $"Scatter Speed Multiplier: {scatterSpeedMultiplier}\n" +
                      $"Corner Proximity: {cornerProximity}\n" +
                      $"Frightened Duration: {frightenedDuration}\n" +
                      $"Frightened Speed Multiplier: {frightenedSpeedMultiplier}");
        }

        public GhostBehaviorType GetInitialBehaviorType()
        {
            return initialBehaviorType;
        }
    }
}
