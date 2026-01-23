using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GhostCardSpace;
using UnityEngine.PlayerLoop;

public class GhostUIDisplay : MonoBehaviour
{

    [Header("Basic Info")]
    public GhostCard cardData;
    public Image ghostImage;
    public Image lifeImage;

    [Header("Traits and Modifiers")]
    public TMP_Text traitText;

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
        // Update Images
        ghostImage.sprite = cardData.Sprite;
        lifeImage.sprite = cardData.Sprite;

        // Update Texts
        traitText.text = cardData.trait.ToString();

        baseSpeedText.text = cardData.baseSpeed.ToString();

        chaseDurationText.text = cardData.chaseDuration.ToString();
        chaseSpeedText.text = cardData.chaseSpeedMultiplier.ToString();
        packProximityText.text = cardData.packProximity.ToString();

        respawnDurationText.text = cardData.respawnDuration.ToString();

        scatterDurationText.text = cardData.scatterDuration.ToString();
        scatterSpeedText.text = cardData.scatterSpeedMultiplier.ToString();
        scatterProximityText.text = cardData.scatterProximity.ToString();

        frightenedSpeedText.text = cardData.frightenedSpeedMultiplier.ToString();
    }

}
