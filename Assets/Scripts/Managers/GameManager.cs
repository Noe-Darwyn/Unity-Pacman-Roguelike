using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Elements")]
    public Ghost[] Ghosts { get; private set; }
    [SerializeField] private Pacman pacman;
    [SerializeField] public Transform pellets;
    [SerializeField] private ExperienceManager experienceManager;
    [SerializeField] private PelletCyclesManager pelletCyclesManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GhostBuilder ghostBuilder;

    [Header("Interface Elements")]
    [SerializeField] private Text pacmanLivesText;
    [SerializeField] private Text ghostLivesText;
    [SerializeField] private Text gameOverText;

    public int scorePacman { get; private set; } = 0;
    public int scoreGhost { get; private set; } = 0;
    public int pacmanLives { get; private set; } = 3;
    public int ghostLives { get; private set; } = 3;

    private int ghostMultiplier = 1;

    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        if (ghostBuilder == null)
        {
            Debug.LogError("[GameManager] Missing reference: ghostBuilder is not assigned.");
            return;
        }

        if (pacman == null)
        {
            Debug.LogError("[GameManager] Missing reference: pacman is not assigned.");
            return;
        }

        ghostBuilder.BuildGhosts(pacman);
        Ghosts = ghostBuilder.ghosts;

        if (Ghosts == null || Ghosts.Length == 0)
        {
            Debug.LogError("[GameManager] Initialization failed: no ghosts were built.");
            return;
        }

        NewGame();
    }

    private void Update()
    {
        if (pacmanLives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
    }

    private void NewGame()
    {
        scoreManager.SetScorePacman(0);
        scoreManager.SetScoreGhost(0);
        scoreManager.SetLevelCleared(0);
        experienceManager.SetExperience();
        SetPacmanLives(3);
        ResetGhostLives();
        CalculateGhostLives();
        NewRound();
    }

    // Réinitialise les vies de tous les fantômes à leur maximum
    private void ResetGhostLives()
    {
        for (int i = 0; i < Ghosts.Length; i++)
        {
            Ghosts[i].lifeManager.ResetLives();
            Ghosts[i].gameObject.SetActive(true);
        }
    }

    private void NewRound()
    {
        gameOverText.enabled = false;
        pelletCyclesManager.InitiateCycles();
        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < Ghosts.Length; i++) {
            // Ne réactiver que les fantômes qui ont encore des vies
            if (Ghosts[i].lifeManager.IsAlive) {
                Ghosts[i].ResetState();
            }
        }

        pacman.ResetState();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;

        for (int i = 0; i < Ghosts.Length; i++) {
            Ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
    }

    private void CalculateGhostLives()
    {
        int totalGhostLives = 0;
        for (int i = 0; i < Ghosts.Length; i++)
        {
            totalGhostLives += Ghosts[i].lifeManager.CurrentLives;
        }
        SetTotalGhostLives(totalGhostLives);
    }

    private void SetTotalGhostLives(int ghostLives)
    {
        this.ghostLives = ghostLives;
        ghostLivesText.text = "x" + ghostLives.ToString();
    }

    private void SetPacmanLives(int pacmanLives)
    {
        this.pacmanLives = pacmanLives;
        pacmanLivesText.text = "x" + pacmanLives.ToString();
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();

        SetPacmanLives(pacmanLives - 1);

        if (pacmanLives > 0) {
            Invoke(nameof(ResetState), 3f);
        } else {
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        scoreManager.SetScorePacman(scoreManager.scorePacman + points);
        ghostMultiplier++;

        // Gérer les vies du fantôme
        bool canRespawn = ghost.lifeManager.OnEaten();
        CalculateGhostLives();
        
        // Met à jour l'affichage des vies de tous les fantômes

        if (canRespawn)
        {
            // Le fantôme a encore des vies, il respawn
            ghost.lifeManager.TriggerRespawn();
        }
        else
        {
            // Le fantôme n'a plus de vies, le désactiver
            ghost.gameObject.SetActive(false);
            
            // Vérifier si tous les fantômes sont morts
            if (AreAllGhostsDead())
            {
                GhostGameOver();
            }
        }

    }

    // Vérifie si tous les fantômes sont définitivement morts (plus de vies)
    private bool AreAllGhostsDead()
    {
        for (int i = 0; i < Ghosts.Length; i++)
        {
            if (Ghosts[i].lifeManager.IsAlive)
            {
                return false;
            }
        }
        return true;
    }

    // Appelé quand tous les fantômes sont morts - Victoire de Pacman
    private void GhostGameOver()
    {
        Debug.Log("Tous les fantômes sont morts! Pacman gagne!");
        gameOverText.text = "PACMAN WINS!";
        gameOverText.enabled = true;
        pacman.gameObject.SetActive(false);
    }

    public void PelletEaten(Pellet pellet, MonoBehaviour collector)
    {
        pellet.gameObject.SetActive(false);
        // Recalculer le ratio de pellets actifs après qu'un pellet a été mangé et la vitesse du cycle en fonction du ratio
        pelletCyclesManager.CalculateActivePelletRatio();
        pelletCyclesManager.CalculatePelletCycleSpeed();
        
        var pacmanCollector = collector.GetComponent<Pacman>();
        var ghostCollector = collector.GetComponent<Ghost>();

        if (pacmanCollector != null)
        {
            scoreManager.SetScorePacman(scoreManager.scorePacman + pellet.points);
        }
        else
        {
            if (ghostCollector != null)
            {
                scoreManager.SetScoreGhost(scoreManager.scoreGhost + pellet.points);
                experienceManager.AddExperience(pellet.points);
            }
        }
     
        if (!HasRemainingPellets())
        {
            foreach (Transform pelletTransform in pellets)
            {
                pelletTransform.gameObject.SetActive(true);
            }
        }
    }

    public void PowerPelletEaten(PowerPellet pellet, MonoBehaviour collector)
    {
        pellet.gameObject.SetActive(false);
        
        var pacmanCollector = collector.GetComponent<Pacman>();
        if (pacmanCollector != null)
        {
            for (int i = 0; i < Ghosts.Length; i++) 
            {
                Ghosts[i].frightened.Enable(Ghosts[i].frightened.duration);
            }

            scoreManager.SetScorePacman(scoreManager.scorePacman + pellet.points);
            CancelInvoke(nameof(ResetGhostMultiplier));
            Invoke(nameof(ResetGhostMultiplier), Ghosts[0].frightened.duration);
        }
        else
        {
            var ghostCollector = collector.GetComponent<Ghost>();
            if (ghostCollector != null)
            {
                scoreManager.SetScoreGhost(scoreManager.scoreGhost + pellet.points);
                experienceManager.AddExperience(pellet.points);
            }
        }

        if (!HasRemainingPellets())
        {
            foreach (Transform pelletTransform in pellets)
            {
                pelletTransform.gameObject.SetActive(true);
            }
        }
        
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

}
