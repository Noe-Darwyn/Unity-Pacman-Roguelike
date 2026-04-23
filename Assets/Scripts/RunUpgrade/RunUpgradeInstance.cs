using TemporaryUpgradeCardSpace;
using UnityEngine;

[System.Serializable]
public class RunUpgradeInstance
{
    public TemporaryUpgradeCard card;
    public int level;

    public RunUpgradeInstance(TemporaryUpgradeCard card)
    {
        this.card = card;
        this.level = 1;
    }

    public void LevelUp()
    {
        if (card == null) return;

        level = Mathf.Min(level + 1, card.maxLevel);
    }

    public bool IsMaxLevel()
    {
        return level >= card.maxLevel;
    }
}
