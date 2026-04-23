using UnityEngine;

public class RunUpgradeController : MonoBehaviour
{
    [SerializeField] private RunUpgradeSelector selector;
    [SerializeField] private RunInventory inventory;
    [SerializeField] private UpgradeChoiceUI ui;
    [SerializeField] private TimeManager timeManager;

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

            Debug.Log($"Player chose: {selected.temporaryUpgradeName}");
            timeManager?.OnTimeResume();

            // plus tard : recalcul gameplay ici
        });
    }
}