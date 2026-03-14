using UnityEngine;

public class PelletCyclesManager : MonoBehaviour
{
    [Header("Pellet Cycle Settings")]
    [Space(10)]
    public float timeToCompletePelletCycle = 30f;
    public float minPelletCycleSpeed = 0.5f; // Vitesse minimale du cycle (lorsque tous les pellets sont actifs)
    public float maxPelletCycleSpeed = 5f; // Vitesse maximale du cycle (lorsque tous les pellets sont mangés)
    
    private float currentPelletCycleTime = 0f;
    private float currentPelletCycleSpeed = 1f;
    
    private int totalPellets; 
    private int totalSuperPellets;
    private float activePelletRatio = 1f; // Ratio de pellets actifs (initialement 100%)

    [Header("Super Pellet Cycle Settings")]
    [Space(10)]
    public float timeToCompleteSuperPelletCycle = 30f; // Temps pour compléter un cycle à 100% de super pellets actifs

    private float currentSuperPelletCycleTime = 0f;

    public void InitiateCycles()
    {
        CountTotalPellets();

        ResetNormalPelletCycle();
        ResetPowerPelletCycle();
    }

    void CountTotalPellets()
    {
        // Compter le nombre total de pellets normaux et de super pellets au début du jeu
        totalPellets = 0;
        totalSuperPellets = 0;
        foreach (Transform pelletTransform in GameManager.Instance.pellets)
        {
            if (pelletTransform.gameObject.GetComponent<PowerPellet>() == null)
            {
                totalPellets++;
            }
            else
            {
                totalSuperPellets++;
            }
        }
        activePelletRatio = 1f;
    }
    void Update()
    {
        UpdatePelletCycleTimer();
        UpdateSuperPelletCycleTimer();
    }

    /// <summary>
    /// CYCLE DES PELLETS NORMAUX :
    //Vitesse du cycle : moins il reste de pellets actifs, plus le cycle avance vite (vitesse calculée en fonction du ratio de pellets actifs).
    //Quand le cycle atteint 0 : tous les pellets normaux sont réactivés, et le cycle recommence.
    /// </summary>
    
    void UpdatePelletCycleTimer()
    {
        if (currentPelletCycleTime >= 0)
        {
            currentPelletCycleTime -= Time.deltaTime * currentPelletCycleSpeed; // Décrémenter le temps du cycle en fonction de la vitesse actuelle
        }
        else 
        {    
            ResetNormalPelletCycle();
        }
    }

        void ResetNormalPelletCycle()
    {   
        // Redémarre le timer
        currentPelletCycleTime = timeToCompletePelletCycle;
        // Réinitialise la vitesse du cycle à sa valeur minimale
        currentPelletCycleSpeed = minPelletCycleSpeed;
        // Réactive tous les pellets normaux
        foreach (Transform pelletTransform in GameManager.Instance.pellets)
            {
                if (pelletTransform.gameObject.GetComponent<PowerPellet>() == null)
                {
                    pelletTransform.gameObject.SetActive(true);
                }
            }
    }


    public float GetCurrentPelletCycleTime()
    {
        return currentPelletCycleTime;
    }

    public float GetTimeToCompletePelletCycle()
    {
        return timeToCompletePelletCycle;
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
    public void CalculatePelletCycleSpeed()
    {
        // Moins il reste de pellets actifs, plus la vitesse augmente.
        currentPelletCycleSpeed = Mathf.Lerp(minPelletCycleSpeed, maxPelletCycleSpeed, 1f - activePelletRatio);
        //Debug.Log($"Cycle Speed Updated: {currentPelletCycleSpeed} (Active Pellet Ratio: {activePelletRatio})");
    }

    public float GetActivePelletRatio()
    {
        return activePelletRatio;
    }

        public float GetCurrentPelletCycleSpeed()
    {
        return currentPelletCycleSpeed;
    }

    /// <summary>
    //CYCLE DES SUPER PELLETS : 
    //Indépendant du cycle de pellets normaux. 
    //La fréquence est uniquement basée sur le temps écoulé depuis le dernier reset des super pellets.
    //Quand le cycle atteint 0 : tous les super pellets sont réactivés, et le cycle recommence.
    /// </summary>
    
    void UpdateSuperPelletCycleTimer()
    {
        if (currentSuperPelletCycleTime >= 0)
        {
            currentSuperPelletCycleTime -= Time.deltaTime; // Le cycle de super pellet avance à vitesse constante
        }
        else 
        {    
            ResetPowerPelletCycle();
        }
    }

    void ResetPowerPelletCycle()
    {
        // Redémarre le timer
        currentSuperPelletCycleTime = timeToCompleteSuperPelletCycle;
        // Réactive tous les power pellets
        foreach (Transform pelletTransform in GameManager.Instance.pellets)
        {
            if (pelletTransform.gameObject.GetComponent<PowerPellet>() != null)
            {
                pelletTransform.gameObject.SetActive(true);
            }
        }
    }

    public float GetCurrentSuperPelletCycleTime()
    {
        return currentSuperPelletCycleTime;
    }
    public float GetTimeToCompleteSuperPelletCycle()
    {
        return timeToCompleteSuperPelletCycle;
    }
    
}