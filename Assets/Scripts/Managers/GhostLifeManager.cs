using UnityEngine;
using System;


// Gère les vies individuelles d'un fantôme à runtime.
// Attaché à chaque instance de Ghost.
public class GhostLifeManager : MonoBehaviour
{
    private Ghost ghost;
    
    [Header("Life Stats")]
    [SerializeField] private int currentLives;
    [SerializeField] private int maxLives;
    
    // Events pour l'UI et autres systèmes
    public event Action<int, int> OnLifeChanged;  // (currentLives, maxLives)
    public event Action OnDeath;                   // Quand le fantôme n'a plus de vies
    public event Action OnRespawn;                 // Quand le fantôme respawn
    
    public int CurrentLives => currentLives;
    public int MaxLives => maxLives;
    public bool IsAlive => currentLives > 0;
    public bool CanRespawn => currentLives > 0;

    private void Awake()
    {
        ghost = GetComponent<Ghost>();
    }


    // Initialise les vies du fantôme (appelé par GhostBuilder)

    public void Initialize(int lives)
    {
        maxLives = lives;
        currentLives = lives;
        OnLifeChanged?.Invoke(currentLives, maxLives);
    }


    //Appelé quand le fantôme est mangé par Pacman.
    // Retourne true si le fantôme peut encore respawn, false sinon.

    public bool OnEaten()
    {
        if (currentLives <= 0) return false;
        
        currentLives--;
        OnLifeChanged?.Invoke(currentLives, maxLives);
        
        if (currentLives > 0)
        {
            return true; // Peut respawn
        }
        else
        {
            OnDeath?.Invoke();
            return false; // Mort définitive
        }
    }


    // Déclenche le respawn du fantôme vers la maison

    public void TriggerRespawn()
    {
        if (!CanRespawn) return;
        
        // Activer le comportement Home qui gère l'animation de sortie
        ghost.home.Enable(ghost.home.duration);
        
        OnRespawn?.Invoke();
    }

    // Réinitialise les vies au maximum (pour NewGame)
    public void ResetLives()
    {
        currentLives = maxLives;
        OnLifeChanged?.Invoke(currentLives, maxLives);
    }
}
