using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Int Upgrade")]
public class IntUpgradeSO : ScriptableObject
{
    public string upgradeName;
    public Int upgradeInt;
    public List<int> upgradeValues;
    public List<int> upgradePrices;
}

