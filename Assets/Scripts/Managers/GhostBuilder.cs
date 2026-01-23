using GhostCardSpace;
using UnityEngine;

public class GhostBuilder : MonoBehaviour
{
    public Ghost[] ghosts;
    public GhostCard[] cardData; 

    void Start()
    {
        InitializeGhostBuilder();
    }

    void InitializeGhostBuilder()
    {
        for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].points = cardData[i].points;

            ghosts[i].movement.speed = cardData[i].baseSpeed;
            ghosts[i].movement.speedMultiplier = cardData[i].baseSpeedMultiplier;

            ghosts[i].chase.duration = cardData[i].chaseDuration;
            ghosts[i].chase.chaseSpeedMultiplier = cardData[i].chaseSpeedMultiplier;
            // ghosts[i].chase.packProximity = cardData[i].packProximity;

            ghosts[i].home.duration = cardData[i].respawnDuration;

            ghosts[i].scatter.duration = cardData[i].scatterDuration;
            ghosts[i].scatter.scatterSpeedMultiplier = cardData[i].scatterSpeedMultiplier;
            // ghosts[i].scatter.cornerProximity = cardData[i].cornerProximity;

            ghosts[i].frightened.frightenedSpeedMultiplier = cardData[i].frightenedSpeedMultiplier;


        }
    }
}
