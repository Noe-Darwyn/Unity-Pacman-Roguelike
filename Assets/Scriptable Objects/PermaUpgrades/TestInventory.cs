using UnityEngine;
using PermanentUpgradeCardSpace;

public class TestInventory : MonoBehaviour
{
    [SerializeField] private PlayerUpgradeInventory inventory;
    [SerializeField] private PermanentUpgradeCard testCard;

    void Start()
    {
        inventory.DisplayInventory();
        
        if (testCard != null)
        {
            Debug.Log($"Current level: {inventory.GetCurrentLevel(testCard)}");
            Debug.Log($"Can purchase level 1: {inventory.CanPurchase(testCard, 1)}");
        }
    }
}