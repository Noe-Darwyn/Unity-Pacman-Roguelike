using System.Collections.Generic;
using UnityEngine;
using TemporaryUpgradeCardSpace;

[CreateAssetMenu(menuName = "Run/Upgrade Database")]
public class RunUpgradeDatabase : ScriptableObject
{
    public List<TemporaryUpgradeCard> allUpgrades;
}