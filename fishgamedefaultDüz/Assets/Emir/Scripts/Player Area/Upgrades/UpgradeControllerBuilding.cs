using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeControllerBuilding : MonoBehaviour
{
    [SerializeField] UpgradeBase[] upgrades;

    [SerializeField] GameObject objToClose;
    [SerializeField] GameObject objToOpen;
    
    Collider coll;

    void Awake()
    {
        coll = GetComponent<Collider>();

        int maxlvl = 0;
        foreach (var upgrade in upgrades)
        {
            if (upgrade.IsUpgradeMax)
            {
                maxlvl++;
            }
            upgrade.OnUpgrade += CheckUpgrades;
        }

        if (maxlvl == upgrades.Length)
        {
            foreach (var upgrade in upgrades)
            {
                upgrade.OnUpgrade -= CheckUpgrades;
            }

            objToClose.SetActive(false);
            objToOpen.SetActive(true);
            coll.enabled = false;
        }
    }

    private void OnDisable() {
        if (coll.enabled)
        {
            foreach (var upgrade in upgrades)
            {
                upgrade.OnUpgrade -= CheckUpgrades;
            }
        }
    }

    private void CheckUpgrades()
    {
        int maxlvl = 0;
        foreach (var upgrade in upgrades)
        {
            if (upgrade.IsUpgradeMax)
            {
                maxlvl++;
            }
        }

        if (maxlvl == upgrades.Length)
        {
            foreach (var upgrade in upgrades)
            {
                upgrade.OnUpgrade -= CheckUpgrades;
            }

            objToClose.SetActive(false);
            objToOpen.SetActive(true);
            coll.enabled = false;
        }
    }
}
