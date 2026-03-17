using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int scorePacman { get; private set; } = 0;
    public int scoreGhost { get; private set; } = 0;
    public int levelCleared { get; private set; } = 0;

    public event Action<int> OnScorePacmanChanged;
    public event Action<int> OnScoreGhostChanged;
    public event Action<int> OnLevelClearedChanged;


    void InitiateScore()
    {
        // Doit initialiser la valeur de base des pellets et super pellets et éventuellement l'impact des upgrades permanentes sur la valeur des pellets et super pellets
        SetScorePacman(0);
        SetScoreGhost(0);
        SetLevelCleared(0);
    }
    void Start()
    {
        InitiateScore();
    }

    public void SetScorePacman(int scorePacman)
    {
        this.scorePacman = scorePacman;
        OnScorePacmanChanged?.Invoke(scorePacman);
    }
    public void SetScoreGhost(int scoreGhost)
    {
        this.scoreGhost = scoreGhost;
        OnScoreGhostChanged?.Invoke(scoreGhost);
    }

    public void SetLevelCleared(int levelCleared)
    {
        this.levelCleared = levelCleared;
        OnLevelClearedChanged?.Invoke(levelCleared);
        //
    }
   

    
    // En cours de jeu :
    // Doit incrémenter le score quand les fantômes mangent des pellets et super pellets en prenant en compte le ratio de pellets actifs pour gérer le multiplicateur de points
    // Le multiplicateur de points retourne au minimum quand les pellets sont tous réactivés et augmente à mesure que les pellets sont mangés

    // Doit incrémenter le score de Pacman quand il mange des pellets et super pellets en prenant en compte le nombre de fantôme mangé pour gérer le multiplicateur de points
    // Le multiplicateur de points pour Pacman doit se réinitialiser après un certain temps sans manger de fantôme

    // A la fin du jeu :
    // Doit passer l'information du score final pour le convertir en ressources à utiliser dans le magasin d'upgrade permanente
    // Doit passer l'information du nombre de levelcleared pour les fantomes pour le convertir en ressources à utiliser dans le magasin d'upgrade permanente

}
