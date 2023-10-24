using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public LoadInt[] upgradesInt;
    public LoadFloat[] upgradesFloat;
    // public UpgradeMaterial[] upgradeMaterials;

    protected override void Awake()
    {
        base.Awake();

        foreach (var upgrade in upgradesInt)
        {
            upgrade.variable.Value = PlayerPrefs.GetInt(upgrade.saveName, upgrade.defaultValue);
        }

        foreach (var upgrade in upgradesFloat)
        {
            upgrade.variable.Value = PlayerPrefs.GetFloat(upgrade.saveName, upgrade.defaultValue);
        }

        /* foreach (var upgrade in upgradeMaterials)
        {
            upgrade.Load();
        } */
    }

}

[System.Serializable]
public class LoadInt
{
    public Int variable;
    public string saveName;
    public int defaultValue;
}

[System.Serializable]
public class LoadFloat
{
    public Float variable;
    public string saveName;
    public float defaultValue;
}