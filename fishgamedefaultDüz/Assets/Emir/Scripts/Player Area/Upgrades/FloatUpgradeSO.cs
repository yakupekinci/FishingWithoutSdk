using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Float Upgrade")]
public class FloatUpgradeSO : ScriptableObject
{
    public string upgradeName;
    public Float upgradeFloat;
    public List<float> upgradeValues;
    public List<int> upgradePrices;
}
