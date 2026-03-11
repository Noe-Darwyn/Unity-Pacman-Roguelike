using System;
using UnityEngine;
using UnityEngine.UI;

public class PelletCycleUI : MonoBehaviour
{
    [SerializeField] private PelletCyclesManager pelletCyclesManager;

    [SerializeField] Text cycleTimeText;
    [SerializeField] Text pelletValueText;

    [SerializeField] Image pelletCycleFiller;  

    private float timeToCompleteCycle;

    void Start()
    {
        timeToCompleteCycle = pelletCyclesManager.GetTimeToCompleteCycle();
    }

    void Update()
    {
        pelletCycleFiller.fillAmount = pelletCyclesManager.GetCurrentCycleTime() / timeToCompleteCycle;
        
        // Affichage du temps restant dans le cycle au format mm:ss
        TimeSpan time = TimeSpan.FromSeconds(pelletCyclesManager.GetCurrentCycleTime());
        cycleTimeText.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

    }

    void UpdatePelletValueText()
    {
        //pelletValueText.text = pelletValue.ToString();
    }
}
