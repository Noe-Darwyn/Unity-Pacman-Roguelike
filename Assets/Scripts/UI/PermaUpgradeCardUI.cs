using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using PermanentUpgradeCardSpace;


/// <summary>
/// Script attaché à chaque carte d'upgrade dans la boutique.
/// Gère l'affichage et l'interaction avec une carte individuelle.
/// </summary>
public class PermaUpgradeCardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image categoryIndicator;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Slider levelProgressBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Image costIcon;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    [Header("Category Colors")]
    [SerializeField] private Color unlockTempColor = new Color(0.608f, 0.349f, 0.714f); // #9B59B6
    [SerializeField] private Color perfectPColor = new Color(0.204f, 0.596f, 0.859f);   // #3498DB
    [SerializeField] private Color imperfectColor = new Color(0.906f, 0.298f, 0.235f);  // #E74C3C
    [SerializeField] private Color pacmanColor = new Color(0.953f, 0.612f, 0.071f);     // #F39C12
    
    private PermanentUpgradeCard currentCard;
    private PlayerUpgradeInventory inventory;
    private int currentLevel;
    
    public event Action<PermanentUpgradeCard, int> OnPurchaseRequested;
    
    
    public void Initialize(PermanentUpgradeCard card, PlayerUpgradeInventory inv)
    {
        if (card == null)
        {
            Debug.LogError("Cannot initialize card with null PermanentUpgradeCard!");
            gameObject.SetActive(false);
            return;
        }
        
        if (inv == null)
        {
            Debug.LogError("Cannot initialize card with null PlayerUpgradeInventory!");
            gameObject.SetActive(false);
            return;
        }
        
        currentCard = card;
        inventory = inv;
        currentLevel = inventory.GetCurrentLevel(card);
        
        if (levelProgressBar != null)
        {
            levelProgressBar.maxValue = card.maxLevel;
            levelProgressBar.value = currentLevel;
        }
        
        if (purchaseButton != null)
        {
            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        }
        
        UpdateVisuals();
        UpdateButtonState();
    }
    
    public void Refresh()
    {
        if (currentCard == null || inventory == null)
            return;
        
        currentLevel = inventory.GetCurrentLevel(currentCard);
        
        if (levelProgressBar != null)
            levelProgressBar.value = currentLevel;
        
        UpdateVisuals();
        UpdateButtonState();
    }
    

    private void UpdateVisuals()
    {
        if (currentCard == null)
            return;
        
        if (categoryIndicator != null)
        {
            categoryIndicator.color = GetCategoryColor(currentCard.category);
        }
        
        if (upgradeIcon != null && currentCard.upgradeSprite != null)
        {
            upgradeIcon.sprite = currentCard.upgradeSprite;
            upgradeIcon.enabled = true;
        }
        else if (upgradeIcon != null)
        {
            upgradeIcon.enabled = false; // Pas de sprite = cacher l'image
        }
        
        if (nameText != null)
        {
            nameText.text = currentCard.upgradeName;
        }
        
        // Description (avec les stats du prochain niveau si pas au max)
        if (descriptionText != null)
        {
            string description = currentCard.upgradeDescription;
            
            if (currentLevel < currentCard.maxLevel)
            {
                int nextLevel = currentLevel + 1;
                description += "\n\n" + GenerateStatsPreview(nextLevel);
            }
            else
            {
                description += "\n\n<color=#2ECC71><b>NIVEAU MAXIMUM ATTEINT</b></color>";
            }
            
            descriptionText.text = description;
        }
        
        if (levelText != null)
        {
            levelText.text = $"Niveau {currentLevel} / {currentCard.maxLevel}";
        }
        
        UpdateCostDisplay();
    }
    
    private void UpdateCostDisplay()
    {
        if (currentLevel >= currentCard.maxLevel)
        {
            // Niveau MAX
            if (costText != null)
                costText.text = "";
            
            if (costIcon != null)
                costIcon.enabled = false;
        }
        else
        {
            // Afficher le coût du prochain niveau
            PermaUpgradeInstance tempInstance = new PermaUpgradeInstance(currentCard, currentLevel);
            int cost = tempInstance.GetNextLevelCost();
            
            if (costText != null)
                costText.text = cost.ToString();
            
            if (costIcon != null)
                costIcon.enabled = true;
        }
    }
    
    private void UpdateButtonState()
    {
        if (purchaseButton == null || buttonText == null)
            return;
        
        if (currentLevel >= currentCard.maxLevel)
        {
            buttonText.text = "MAX";
            purchaseButton.interactable = false;
            return;
        }
        
        // Vérifier si on peut acheter
        int nextLevel = currentLevel + 1;
        bool canPurchase = inventory.CanPurchase(currentCard, nextLevel);
        
        if (canPurchase)
        {
            buttonText.text = currentLevel == 0 ? "ACHETER" : "AMÉLIORER";
            purchaseButton.interactable = true;
        }
        else
        {
            buttonText.text = "INSUFFISANT";
            purchaseButton.interactable = false;
        }
    }
    
    private string GenerateStatsPreview(int level)
    {
        if (currentCard == null || level <= 0 || level > currentCard.maxLevel)
            return "";
        
        PermaUpgradeInstance preview = new PermaUpgradeInstance(currentCard, level);
        string stats = $"<b>Au niveau {level} :</b>";
        
        // Ajouter les stats non-nulles
        if (preview.GetHealthIncrease() != 0)
            stats += $"\n• Santé : +{preview.GetHealthIncrease()}";
        
        if (preview.GetPointsDecrease() != 0)
            stats += $"\n• Points : {preview.GetPointsDecrease()}";
        
        if (preview.GetBaseSpeedIncrease() != 0)
            stats += $"\n• Vitesse : {FormatStat(preview.GetBaseSpeedIncrease())}";
        
        if (preview.GetBaseSpeedMultiplierIncrease() != 0)
            stats += $"\n• Multiplicateur vitesse : {FormatStat(preview.GetBaseSpeedMultiplierIncrease())}";
        
        if (preview.GetChaseDurationIncrease() != 0)
            stats += $"\n• Durée chase : +{preview.GetChaseDurationIncrease()}s";
        
        if (preview.GetChaseSpeedMultiplierIncrease() != 0)
            stats += $"\n• Vitesse chase : {FormatStat(preview.GetChaseSpeedMultiplierIncrease())}";
        
        if (preview.GetRespawnDurationDecrease() != 0)
            stats += $"\n• Durée respawn : -{preview.GetRespawnDurationDecrease()}s";
        
        if (preview.GetScatterDurationIncrease() != 0)
            stats += $"\n• Durée scatter : +{preview.GetScatterDurationIncrease()}s";
        
        if (preview.GetScatterSpeedMultiplierIncrease() != 0)
            stats += $"\n• Vitesse scatter : {FormatStat(preview.GetScatterSpeedMultiplierIncrease())}";
        
        if (preview.GetFrightenedDurationDecrease() != 0)
            stats += $"\n• Durée frightened : -{preview.GetFrightenedDurationDecrease()}s";
        
        if (preview.GetFrightenedSpeedMultiplierIncrease() != 0)
            stats += $"\n• Vitesse frightened : {FormatStat(preview.GetFrightenedSpeedMultiplierIncrease())}";
        
        return stats;
    }
    
    private string FormatStat(float value)
    {
        if (value > 0)
            return $"+{value:F2}";
        else
            return value.ToString("F2");
    }
    
    private Color GetCategoryColor(UpgradeCategory category)
    {
        switch (category)
        {
            case UpgradeCategory.UnlockTempUpgrade:
                return unlockTempColor;
            case UpgradeCategory.PerfectPUpgrade:
                return perfectPColor;
            case UpgradeCategory.ImperfectUpgrade:
                return imperfectColor;
            case UpgradeCategory.PacmanUpgrade:
                return pacmanColor;
            default:
                return Color.gray;
        }
    }
    
    private void OnPurchaseButtonClicked()
    {
        if (currentCard == null || inventory == null)
            return;
        
        if (currentLevel >= currentCard.maxLevel)
        {
            Debug.LogWarning($"{currentCard.upgradeName} is already at max level!");
            return;
        }
        
        int nextLevel = currentLevel + 1;
        
        // Déclencher l'event (le shop UI gérera l'achat)
        OnPurchaseRequested?.Invoke(currentCard, nextLevel);
    }
}