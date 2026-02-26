using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using PermanentUpgradeCardSpace;

/// <summary>
/// Gestionnaire principal de la boutique d'upgrades.
/// Génère les cartes, gère les achats, filtre par catégorie.
/// </summary>
public class PermaUpgradeShopUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PermaUpgradeDatabase database;
    [SerializeField] private PlayerUpgradeInventory inventory;
    [SerializeField] private PermanentUpgradeManager upgradeManager;
    
    [Header("UI Elements")]
    [SerializeField] private PermaUpgradeCardUI cardPrefab;
    [SerializeField] private Transform upgradesContainer;
    [SerializeField] private TextMeshProUGUI currencyText;
    
    [Header("Category Tabs")]
    [SerializeField] private Toggle tabAll;
    [SerializeField] private Toggle tabUnlockTemp;
    [SerializeField] private Toggle tabPerfectP;
    [SerializeField] private Toggle tabImperfect;
    [SerializeField] private Toggle tabPacman;
    
    // État actuel
    private UpgradeCategory? currentFilter = null; // null = afficher tout
    private List<PermaUpgradeCardUI> instantiatedCards = new List<PermaUpgradeCardUI>();
    
    
    void Start()
    {
        if (!ValidateReferences())
            return;
        
        SetupTabs();
        if (tabAll != null)
            tabAll.isOn = true; // Par défaut, afficher tout
        
        GenerateShopUI(null);
        
        UpdateCurrencyDisplay();
    }
    
    private bool ValidateReferences()
    {
        bool isValid = true;
        
        if (database == null)
        {
            Debug.LogError("PermaUpgradeShopUI: Database is not assigned!");
            isValid = false;
        }
        
        if (inventory == null)
        {
            Debug.LogError("PermaUpgradeShopUI: Inventory is not assigned!");
            isValid = false;
        }
        
        if (upgradeManager == null)
        {
            Debug.LogError("PermaUpgradeShopUI: UpgradeManager is not assigned!");
            isValid = false;
        }
        
        if (cardPrefab == null)
        {
            Debug.LogError("PermaUpgradeShopUI: CardPrefab is not assigned!");
            isValid = false;
        }
        
        if (upgradesContainer == null)
        {
            Debug.LogError("PermaUpgradeShopUI: UpgradesContainer is not assigned!");
            isValid = false;
        }
        
        if (currencyText == null)
        {
            Debug.LogWarning("PermaUpgradeShopUI: CurrencyText is not assigned!");
        }
        
        return isValid;
    }
    
    private void SetupTabs()
    {
        if (tabAll != null)
            tabAll.onValueChanged.AddListener((isOn) => { 
                if(isOn) 
                    OnCategoryTabClicked(null); 
                else 
                    Debug.Log("All tab deselected (should not happen if using ToggleGroup)");
            });
        
        if (tabUnlockTemp != null)
            tabUnlockTemp.onValueChanged.AddListener((isOn) => {
                if(isOn) 
                    OnCategoryTabClicked(UpgradeCategory.UnlockTempUpgrade); 
                else 
                    Debug.Log("UnlockTemp tab deselected (should not happen if using ToggleGroup)");
            });
        
        if (tabPerfectP != null)
            tabPerfectP.onValueChanged.AddListener((isOn) => {
                if(isOn) 
                    OnCategoryTabClicked(UpgradeCategory.PerfectPUpgrade); 
                else 
                    Debug.Log("PerfectP tab deselected (should not happen if using ToggleGroup)");
            });
        
        if (tabImperfect != null)
            tabImperfect.onValueChanged.AddListener((isOn) => {
                if(isOn) 
                    OnCategoryTabClicked(UpgradeCategory.ImperfectUpgrade); 
                else 
                    Debug.Log("Imperfect tab deselected (should not happen if using ToggleGroup)");
            });
        
        if (tabPacman != null)
            tabPacman.onValueChanged.AddListener((isOn) => {
                if(isOn) 
                    OnCategoryTabClicked(UpgradeCategory.PacmanUpgrade); 
                else 
                    Debug.Log("Pacman tab deselected (should not happen if using ToggleGroup)");
            });
    }
    
    
    public void GenerateShopUI(UpgradeCategory? category)
    {
        ClearShop();
        
        List<PermanentUpgradeCard> upgradesToDisplay;
        
        if (category == null)  // Afficher tout
        {
            upgradesToDisplay = database.GetAllUpgrades();
            Debug.Log($"Displaying all upgrades ({upgradesToDisplay.Count} total)");
        }
        else // Filtrer par catégorie
        {
            upgradesToDisplay = database.GetUpgradesByCategory(category.Value);
            Debug.Log($"Displaying {category.Value} upgrades ({upgradesToDisplay.Count} cards)");
        }
        
        // Créer une carte pour chaque upgrade
        foreach (PermanentUpgradeCard upgrade in upgradesToDisplay)
        {
            if (upgrade == null)
            {
                Debug.LogWarning("Skipping null upgrade in database");
                continue;
            }
            
            PermaUpgradeCardUI cardUI = Instantiate(cardPrefab, upgradesContainer);
            
            cardUI.Initialize(upgrade, inventory);
            // S'abonner à l'event d'achat
            cardUI.OnPurchaseRequested += OnUpgradePurchased;
            instantiatedCards.Add(cardUI);
        }
        
        // Stocker le filtre actuel
        currentFilter = category;
        
        if (instantiatedCards.Count == 0)
        {
            Debug.LogWarning($"No upgrades to display for category: {category}");
        }

        ResetScrollPosition();
    }

    private void ResetScrollPosition()
    {
        if (upgradesContainer != null)
        {
            ScrollRect scrollRect = upgradesContainer.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 1f; // Remonter en haut
            }
        }
    }

    private void ClearShop()
    {
        // Se désabonner et détruire chaque carte
        foreach (PermaUpgradeCardUI card in instantiatedCards)
        {
            if (card != null)
            {
                card.OnPurchaseRequested -= OnUpgradePurchased;
                Destroy(card.gameObject);
            }
        }
        
        instantiatedCards.Clear();
    }
    
    /// Appelé quand une carte émet l'event OnPurchaseRequested
    private void OnUpgradePurchased(PermanentUpgradeCard card, int targetLevel)
    {
        Debug.Log($"Purchase requested: {card.upgradeName} level {targetLevel}");
        
        // Tenter l'achat dans l'inventory
        bool success = inventory.PurchaseUpgrade(card, targetLevel);
        
        if (success)
        {
            Debug.Log($"Purchase successful!");
            
            upgradeManager.AddOrUpgradeCard(card, targetLevel);
            
            UpdateCurrencyDisplay();
            RefreshAllCards();
            
            // TODO (optionnel) : Effet visuel (particules, animation)
        }
        else
        {
            // Achat échoué
            Debug.LogWarning($"Purchase failed!");
            
            // TODO (optionnel) : Afficher un message d'erreur
        }
    }
    
    private void UpdateCurrencyDisplay()
    {
        if (currencyText != null)
        {
            currencyText.text = inventory.Currency.ToString("N0"); // Format avec séparateurs (ex: 1,234)
        }
    }
    
    private void RefreshAllCards()
    {
        foreach (PermaUpgradeCardUI card in instantiatedCards)
        {
            if (card != null)
            {
                card.Refresh();
            }
        }
    }
    
    
    public void OnCategoryTabClicked(UpgradeCategory? category)
    {
        Debug.Log($"Tab clicked: {(category == null ? "All" : category.ToString())}");
        
        // Regénérer l'UI avec le nouveau filtre
        GenerateShopUI(category);
    }
    
    
    public void RefreshShop()
    {
        GenerateShopUI(currentFilter);
        UpdateCurrencyDisplay();
    }
    
    /// Ajoute de la monnaie au joueur (pour debug/tests)
    public void AddCurrency(int amount)
    {
        inventory.AddCurrency(amount);
        UpdateCurrencyDisplay();
        RefreshAllCards();
    }
    

    // Important pour éviter gros lags    
    void OnDestroy()
    {
        // Se désabonner de tous les events pour éviter les fuites mémoire
        foreach (PermaUpgradeCardUI card in instantiatedCards)
        {
            if (card != null)
            {
                card.OnPurchaseRequested -= OnUpgradePurchased;
            }
        }
    }
}