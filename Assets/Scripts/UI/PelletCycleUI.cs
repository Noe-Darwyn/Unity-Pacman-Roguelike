using System;
using UnityEngine;
using UnityEngine.UI;

public class PelletCycleUI : MonoBehaviour
{
    [SerializeField] private PelletCyclesManager pelletCyclesManager;

    [Header("Pellet UI Elements")]
    [SerializeField] Text pelletCycleTimeText;
    [SerializeField] Image pelletCycleFiller; 
    private float timeToCompletePelletCycle;

    [Header("Super Pellet UI Elements")]
    [SerializeField] Text superPelletCycleTimeText;
    [SerializeField] Image superPelletCycleFiller;  
    private float timeToCompleteSuperPelletCycle;

    void Start()
    {
        timeToCompletePelletCycle = pelletCyclesManager.GetTimeToCompletePelletCycle();
        timeToCompleteSuperPelletCycle = pelletCyclesManager.GetTimeToCompleteSuperPelletCycle();
    }

    void Update()
    {
        UpdatePelletUI();
        UpdateSuperPelletUI();
    }
    void UpdatePelletUI()
    {
        pelletCycleFiller.fillAmount = pelletCyclesManager.GetCurrentPelletCycleTime() / timeToCompletePelletCycle;
        
        // Affichage du temps restant dans le cycle au format mm:ss
        TimeSpan time = TimeSpan.FromSeconds(pelletCyclesManager.GetCurrentPelletCycleTime());
        pelletCycleTimeText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

    }

    void UpdateSuperPelletUI()
    {
        superPelletCycleFiller.fillAmount = pelletCyclesManager.GetCurrentSuperPelletCycleTime() / timeToCompleteSuperPelletCycle;
        
        // Affichage du temps restant dans le cycle au format mm:ss
        TimeSpan time = TimeSpan.FromSeconds(pelletCyclesManager.GetCurrentSuperPelletCycleTime());
        superPelletCycleTimeText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
    }
}
