using UnityEngine;
using PermanentUpgradeCardSpace;

public class TestUpgradeSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PermaUpgradeDatabase database;
    [SerializeField] private PlayerUpgradeInventory inventory;

    void Start()
    {
        Debug.Log("========== UPGRADE SYSTEM TEST ==========");
        
        // Test 1 : Valider la database
        Debug.Log("\n--- TEST 1: Database Validation ---");
        database.ValidateDatabase();
        
        // Test 2 : Afficher la database
        Debug.Log("\n--- TEST 2: Database Content ---");
        database.DisplayDatabase();
        
        // Test 3 : Afficher l'inventaire initial
        Debug.Log("\n--- TEST 3: Initial Inventory ---");
        inventory.DisplayInventory();
        
        // Test 4 : Tester un achat
        Debug.Log("\n--- TEST 4: Purchase Test ---");
        PermanentUpgradeCard speedDemon = database.GetUpgradeByName("Test Speed Demon");
        
        if (speedDemon != null)
        {
            Debug.Log($"Attempting to purchase {speedDemon.upgradeName} level 1...");
            bool success = inventory.PurchaseUpgrade(speedDemon, 1);
            
            if (success)
            {
                Debug.Log("✅ Purchase successful!");
                inventory.DisplayInventory();
                
                // Test 5 : Créer une instance et vérifier les valeurs
                Debug.Log("\n--- TEST 5: Instance Values ---");
                PermaUpgradeInstance instance = new PermaUpgradeInstance(speedDemon, 1);
                Debug.Log($"Speed bonus at level 1: {instance.GetBaseSpeedIncrease()}");
                Debug.Log($"Speed multiplier bonus at level 1: {instance.GetBaseSpeedMultiplierIncrease()}");
            }
            else
            {
                Debug.LogError("❌ Purchase failed!");
            }
        }
        else
        {
            Debug.LogError("Speed Demon not found in database!");
        }
        
        Debug.Log("\n========== TEST COMPLETE ==========");
    }
}