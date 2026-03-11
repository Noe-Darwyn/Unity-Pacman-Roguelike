using UnityEngine;
using UnityEngine.UI;

public class PelletCyclesManager : MonoBehaviour
{
    private int totalPellets; // Nombre total de pellets
    private float activePelletRatio = 1f; // Ratio de pellets actifs (initialement 100%)

    public float timeToCompleteCycle = 30f; // Temps pour compléter un cycle à 100% de pellets actifs
    public float currentCycleTime = 0f; // Temps écoulé dans le cycle actuel

    public float minCycleSpeed = 0.5f; // Vitesse minimale du cycle (lorsque tous les pellets sont mangés)
    public float maxCycleSpeed = 5f; // Vitesse maximale du cycle (lorsque tous les pellets sont actifs)
    public float currentCycleSpeed = 1f; // Vitesse actuelle du cycle

    void Start()
    {
         foreach (Transform pelletTransform in GameManager.Instance.pellets)
             {
                if (pelletTransform.gameObject.GetComponent<PowerPellet>() == null)
                {
                    totalPellets++;
                }
             }
        // Au début du round, tous les pellets normaux sont actifs: vitesse minimale.
        activePelletRatio = 1f;
        currentCycleSpeed = minCycleSpeed;
        ResetCycle();
    }

    void Update()
    {
        if (currentCycleTime >= 0)
        {
            currentCycleTime -= Time.deltaTime * currentCycleSpeed; // Décrémenter le temps du cycle en fonction de la vitesse actuelle
        }
        else // Effectuer une action à la fin du cycle, par exemple, réinitialiser les pellets
        {    
            ResetCycle();
            
            foreach (Transform pelletTransform in GameManager.Instance.pellets)
            {
                if (pelletTransform.gameObject.GetComponent<PowerPellet>() == null)
                {
                    pelletTransform.gameObject.SetActive(true);
                }
            }

            // Ne pas recalculer ici pour éviter les calls en Update.
            // Le recalcul est fait uniquement quand un pellet est mangé.
            activePelletRatio = 1f;
            currentCycleSpeed = minCycleSpeed;
        }
    }

    void ResetCycle()
    {
        currentCycleTime = timeToCompleteCycle;
    }

    public float GetCurrentCycleTime()
    {
        return currentCycleTime;
    }

    public float GetTimeToCompleteCycle()
    {
        return timeToCompleteCycle;
    }

    public void CalculateActivePelletRatio()
    {
        int activePellets = 0;

        foreach (Transform pelletTransform in GameManager.Instance.pellets)
             {
                if (pelletTransform.gameObject.GetComponent<PowerPellet>() == null && pelletTransform.gameObject.activeSelf)
                {
                    activePellets++;
                }
             }

        activePelletRatio = totalPellets > 0 ? (float)activePellets / totalPellets : 0f;
    }

    public float GetActivePelletRatio()
    {
        return activePelletRatio;
    }

    public void CalculateCycleSpeed()
    {
        // Moins il reste de pellets actifs, plus la vitesse augmente.
        currentCycleSpeed = Mathf.Lerp(minCycleSpeed, maxCycleSpeed, 1f - activePelletRatio);
        Debug.Log($"Cycle Speed Updated: {currentCycleSpeed} (Active Pellet Ratio: {activePelletRatio})");
    }

    public float GetCurrentCycleSpeed()
    {
        return currentCycleSpeed;
    }
}