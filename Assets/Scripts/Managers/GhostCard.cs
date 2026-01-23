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
        public int health;
        public int points = 0;

        [Header("Traits and Modifiers")]
        public GhostTrait trait;

        public enum GhostTrait
        {
            Imp,
            Agr,
            Amb,
            Dup
        }
        
        [Header("Movement and Behavior Stats")]
        [Header("Base Stats")]
        public int baseSpeed;
        // Not displayed, might be used for calculations
        public int baseSpeedMultiplier;

        [Header("Chase Stats")]
        public int chaseDuration;
        public int chaseSpeedMultiplier;
        public int packProximity;

        [Header("Spawn Stats")]  
        public int respawnDuration;

        [Header("Scatter Stats")]
        public int scatterDuration;
        public int scatterSpeedMultiplier;
        public int scatterProximity;
        // Not displayed, used once the UI setup is done
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
            Debug.Log($"Health: {health}\n" +
                      $"Points: {points}\n" +
                      $"Trait: {trait}\n" +
                      $"Base Speed: {baseSpeed}\n" +
                      $"Base Speed Multiplier: {baseSpeedMultiplier}\n" +
                      $"Chase Duration: {chaseDuration}\n" +
                      $"Chase Speed Multiplier: {chaseSpeedMultiplier}\n" +
                      $"Pack Proximity: {packProximity}\n" +
                      $"Respawn Duration: {respawnDuration}\n" +
                      $"Scatter Duration: {scatterDuration}\n" +
                      $"Scatter Speed Multiplier: {scatterSpeedMultiplier}\n" +
                      $"Scatter Proximity: {scatterProximity}\n" +
                      $"Scatter Corner: {scatterCorner}\n" +
                      $"Frightened Speed Multiplier: {frightenedSpeedMultiplier}");
        }
    }
}
