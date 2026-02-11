using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("Game Elements")]
    public Ghost[] Ghosts { get; private set; }
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    [SerializeField] private ExperienceManager experienceManager;

    [Header("Ghost Spawning")]
    [SerializeField] private Ghost ghostPrefab;
    [SerializeField] private Transform ghostParent;
    [SerializeField] private Transform ghostHomeInside;
    [SerializeField] private Transform ghostHomeOutside;

    [Header("Interface Elements")]
    [SerializeField] private Text scorePacmanText;
    [SerializeField] private Text scoreGhostText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text gameOverText;

    public int scorePacman { get; private set; } = 0;
    public int scoreGhost { get; private set; } = 0;
    public int lives { get; private set; } = 3;

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
        GhostBuilder ghostBuilder = FindObjectOfType<GhostBuilder>();
        if (ghostBuilder != null)
        {
            ghostBuilder.BuildGhosts(ghostPrefab, ghostParent, pacman, ghostHomeInside, ghostHomeOutside);
            Ghosts = ghostBuilder.ghosts;
        }
        else
        {
            Debug.LogError("GameManager: GhostBuilder not found in the scene!");
        }

        NewGame();
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown) {
            NewGame();
        }
    }

    private void NewGame()
    {
        SetScorePacman(0);
        SetScoreGhost(0);
        experienceManager.SetExperience();
        SetLives(3);
        NewRound();
        
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < Ghosts.Length; i++) {
            Ghosts[i].ResetState();
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

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScorePacman(int scorePacman)
    {
        this.scorePacman = scorePacman;
        scorePacmanText.text = scorePacman.ToString().PadLeft(2, '0');
    }
    private void SetScoreGhost(int scoreGhost)
    {
        this.scoreGhost = scoreGhost;
        scoreGhostText.text = scoreGhost.ToString().PadLeft(2, '0');
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();

        SetLives(lives - 1);

        if (lives > 0) {
            Invoke(nameof(ResetState), 3f);
        } else {
            GameOver();
        }
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScorePacman(scorePacman + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet, MonoBehaviour collector)
    {
        pellet.gameObject.SetActive(false);
        
        var pacmanCollector = collector.GetComponent<Pacman>();
        var ghostCollector = collector.GetComponent<Ghost>();

        if (pacmanCollector != null)
        {
            SetScorePacman(scorePacman + pellet.points);
        }
        else
        {
            if (ghostCollector != null)
            {
                SetScoreGhost(scoreGhost + pellet.points);
                experienceManager.AddExperience(pellet.points);
            }
        }
     
        if (!HasRemainingPellets())
        {
            foreach (Transform pelletTransform in pellets)
            {
                pelletTransform.gameObject.SetActive(true);
            }
            //pacman.gameObject.SetActive(false);
            //Invoke(nameof(NewRound), 3f);
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

            SetScorePacman(scorePacman + pellet.points);
            CancelInvoke(nameof(ResetGhostMultiplier));
            Invoke(nameof(ResetGhostMultiplier), Ghosts[0].frightened.duration);
        }
        else
        {
            var ghostCollector = collector.GetComponent<Ghost>();
            if (ghostCollector != null)
            {
                SetScoreGhost(scoreGhost + pellet.points);
                experienceManager.AddExperience(pellet.points);
            }
        }

        if (!HasRemainingPellets())
        {
            foreach (Transform pelletTransform in pellets)
            {
                pelletTransform.gameObject.SetActive(true);
            }
            //pacman.gameObject.SetActive(false);
            //Invoke(nameof(NewRound), 3f);
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
