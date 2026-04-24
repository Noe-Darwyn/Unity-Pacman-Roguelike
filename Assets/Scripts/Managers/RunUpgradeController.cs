using UnityEngine;

public class RunUpgradeController : MonoBehaviour
{
    [SerializeField] private RunUpgradeSelector selector;
    [SerializeField] private RunInventory inventory;
    [SerializeField] private UpgradeChoiceUI ui;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private RunUpgradeManager upgradeManager;
    [SerializeField] private GhostBuilder ghostBuilder;

    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            TriggerUpgradeChoice();
        }
    }
    */

    public void TriggerUpgradeChoice()
    {
        Debug.Log("Triggering upgrade choice...");
        timeManager?.OnTimeStop();

        var choices = selector.GetRandomChoices(3);

        ui.Show(choices, (selected) =>
        {
            inventory.AddOrUpgrade(selected);

            upgradeManager.RecalculateStats();

            ghostBuilder.ReapplyStats();

            Debug.Log($"Player chose: {selected.temporaryUpgradeName}");

            timeManager?.OnTimeResume();
        });
    }
}