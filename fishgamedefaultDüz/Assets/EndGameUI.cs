using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] GameObject endGameArea;
    [SerializeField] UpgradeBase[] upgrades;

    bool endGameActivated;

    bool hasEndGameShown;

    private void Start()
    {
        hasEndGameShown = PlayerPrefs.GetInt("hasEndGameShown", 0) == 1;

        if (hasEndGameShown)
        {
            Destroy(gameObject);
            return;
        }

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
            endGameActivated = true;
            ShowEndGameArea();
        }
    }

    private void OnDisable() {
        if (!endGameActivated)
        {
            foreach (var upgrade in upgrades)
            {
                upgrade.OnUpgrade -= CheckUpgrades;
            }
        }
    }

    private void ShowEndGameArea()
    {
        endGameArea.SetActive(true);
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
            endGameActivated = true;
            ShowEndGameArea();
        }
    }

}
