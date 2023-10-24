using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeInt : UpgradeBase
{
    public override long currentPrice =>upgradeInfo.upgradePrices[Mathf.Min(upgradeLevel + 1, upgradeInfo.upgradePrices.Count - 1)];
    public IntUpgradeSO upgradeInfo;
    int upgradeLevel;
    bool isLoaded;

    public override bool IsUpgradeMax =>
    isLoaded ? upgradeLevel + 1 >= upgradeInfo.upgradePrices.Count
    : PlayerPrefs.GetInt(upgradeInfo.upgradeName, 0) + 1 >= upgradeInfo.upgradePrices.Count;


    protected void Awake()
    {
        upgradeLevel = PlayerPrefs.GetInt(upgradeInfo.upgradeName, 0);
        SetLvlText(upgradeLevel);
        SetMaxLvlText(upgradeInfo.upgradePrices.Count);
        if (upgradeLevel + 1 < upgradeInfo.upgradePrices.Count)
        {
            SetUpgradeInfoText(upgradeInfo.upgradeValues[upgradeLevel + 1] - upgradeInfo.upgradeValues[upgradeLevel], upgradeInfo.upgradeValues[upgradeLevel], upgradeInfo.upgradeValues[upgradeLevel + 1]);
        }

        isLoaded = true;
    }

    public void UpgradeByGold()
    {
        if (PlayerManager.Instance.gold.Value >= upgradeInfo.upgradePrices[upgradeLevel + 1])
        {
            AudioManager.Instance.PlayUpgradeUIClip();
            PlayerManager.Instance.ReduceGold(upgradeInfo.upgradePrices[upgradeLevel + 1]);
            Upgrade();
        }
    }


    public void UpgradeByWatchAd()
    {
        AudioManager.Instance.PlayUISoundAtGameCam(); 
        Upgrade();
      /*   AdManager.Instance.ShowAd(Upgrade, null); */
    }

    private void Upgrade()
    {
        upgradeLevel++;
        PlayerPrefs.SetInt(upgradeInfo.upgradeName, upgradeLevel);
        int value = upgradeInfo.upgradeValues[upgradeLevel];
        upgradeInfo.upgradeInt.Value = value;
        PlayerPrefs.SetInt(upgradeInfo.upgradeName + "val", value);
        SetLvlText(upgradeLevel);

        if (upgradeLevel + 1 < upgradeInfo.upgradePrices.Count)
        {
            SetPriceText(upgradeInfo.upgradePrices[upgradeLevel + 1]);
            SetUpgradeInfoText(upgradeInfo.upgradeValues[upgradeLevel + 1] - upgradeInfo.upgradeValues[upgradeLevel], upgradeInfo.upgradeValues[upgradeLevel], upgradeInfo.upgradeValues[upgradeLevel + 1]);
        }
        else
        {
            ShowMaxTMP();
        }
        OnUpgrade?.Invoke();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (upgradeLevel + 1 < upgradeInfo.upgradePrices.Count)
        {
            long price = upgradeInfo.upgradePrices[upgradeLevel + 1];
            SetPriceText(price);
        }
        else
        {
            ShowMaxTMP();
        }


    }

}
