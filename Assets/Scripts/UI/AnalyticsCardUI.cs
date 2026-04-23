using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GhostCardSpace;
using UnityEngine.PlayerLoop;

public class AnalyticsCardUI : MonoBehaviour
{
    public Ghost ghost;
    
    [Header("Basic Info")]
    public Image ghostImage;
    public TMP_Text ghostNameText;
    public TMP_Text descriptionText;

    [Header("Lives and Points")]
    public TMP_Text lives;
    public TMP_Text points;
    public TMP_Text traitText;
    [Header("Traits and Modifiers")]
    

    [Header("Movement and Behavior Stats")]
    [Header("Base Stats")]
    public TMP_Text baseSpeedText;

    [Header("Chase Stats")]
    public TMP_Text chaseDurationText;
    public TMP_Text chaseSpeedText;
    public TMP_Text packProximityText;
    
    [Header("Spawn Stats")]
    public TMP_Text respawnDurationText;

    [Header("Scatter Stats")]
    public TMP_Text scatterDurationText;
    public TMP_Text scatterSpeedText;
    public TMP_Text scatterProximityText;

    [Header("Frightened Stats")]
    public TMP_Text frightenedSpeedText;

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        // Update Basic Info
        ghostImage.sprite = ghost.ghostSprite;
        ghostNameText.text = ghost.ghostName;
        descriptionText.text = ghost.ghostDescription;
        
        // Update Lives and Points
        lives.text = ghost.lifeManager.currentLives.ToString();
        points.text = ghost.points.ToString();
        traitText.text = ghost.trait;
        
        // Update Base Stats
        baseSpeedText.text = ghost.movement.speed.ToString();

        // Update Chase Stats
        chaseDurationText.text = ghost.chase.duration.ToString();
        chaseSpeedText.text = ghost.chase.chaseSpeedMultiplier.ToString();
        //packProximityText.text = ghost.movement.packProximity.ToString();

        // Update Frightened Stats
        frightenedSpeedText.text = ghost.frightened.duration.ToString();
        
        // Update Spawn Stats
        //respawnDurationText.text = .ToString();

        // Update Scatter Stats
        scatterDurationText.text = ghost.scatter.duration.ToString();
        scatterSpeedText.text = ghost.scatter.scatterSpeedMultiplier.ToString();
        //scatterProximityText.text = ghost.scatter.scatterProximity.ToString();

        
    }

}
