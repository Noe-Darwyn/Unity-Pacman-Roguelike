using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManagerUI : MonoBehaviour
{
    [SerializeField] private Text scorePacmanText;
    [SerializeField] private Text scoreGhostText;
    [SerializeField] private Text levelClearedText;

    private ScoreManager scoreManager;

    void Start()
    {
        // Récupère le ScoreManager (adapter si vous n'avez pas de singleton)
        scoreManager = FindObjectOfType<ScoreManager>();
        
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found!");
            return;
        }

        // S'abonne aux événements
        scoreManager.OnScorePacmanChanged += UpdateScorePacmanUI;
        scoreManager.OnScoreGhostChanged += UpdateScoreGhostUI;
        scoreManager.OnLevelClearedChanged += UpdateLevelClearedUI;

        // Affiche les valeurs initiales
        UpdateScorePacmanUI(scoreManager.scorePacman);
        UpdateScoreGhostUI(scoreManager.scoreGhost);
        UpdateLevelClearedUI(scoreManager.levelCleared);
    }

    private void UpdateScorePacmanUI(int newScore)
    {
        if (scorePacmanText != null)
            scorePacmanText.text = newScore.ToString();
    }

    private void UpdateScoreGhostUI(int newScore)
    {
        if (scoreGhostText != null)
            scoreGhostText.text = newScore.ToString();
    }

    private void UpdateLevelClearedUI(int levelCleared)
    {
        if (levelClearedText != null)
            levelClearedText.text = levelCleared.ToString();
    }

    // ⚠️ IMPORTANT : Se désabonner des événements pour éviter les memory leaks
    void OnDestroy()
    {
        if (scoreManager != null)
        {
            scoreManager.OnScorePacmanChanged -= UpdateScorePacmanUI;
            scoreManager.OnScoreGhostChanged -= UpdateScoreGhostUI;
            scoreManager.OnLevelClearedChanged -= UpdateLevelClearedUI;
        }
    }
}
