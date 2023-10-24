using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkerUpgradeData
{
    public string upgradeName;
    public Float workerSpeed;
    public Int workerStack;
    public List<float> speedValues;
    public List<int> stackValues;
    public List<int> upgradePrices;
}

public class UpgradeWorker : UpgradeBase
{
    public override long currentPrice => upgradeInfo.upgradePrices[Mathf.Min(upgradeLevel + 1, upgradeInfo.upgradePrices.Count - 1)];
    public WorkerUpgradeData upgradeInfo;
    public GameObject worker;
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
        isLoaded = true;
    }

    public void UpgradeByGold()
    {
        if (PlayerManager.Instance.gold.Value >= upgradeInfo.upgradePrices[upgradeLevel + 1])
        {
            PlayerManager.Instance.ReduceGold(upgradeInfo.upgradePrices[upgradeLevel + 1]);
            AudioManager.Instance.PlayUpgradeUIClip();
            Upgrade();
        }
    }

    public void UpgradeByWatchAd()
    {
        AudioManager.Instance.PlayUISoundAtGameCam();
       /*  AdManager.Instance.ShowAd(Upgrade, null); */
       Upgrade();
    }

    private void Upgrade()
    {
        upgradeLevel++;
        PlayerPrefs.SetInt(upgradeInfo.upgradeName, upgradeLevel);
        if (upgradeLevel == 1)
        {
            worker.SetActive(true);
        }
        var speedVal = upgradeInfo.speedValues[upgradeLevel];
        upgradeInfo.workerSpeed.Value = speedVal;
        PlayerPrefs.SetFloat(upgradeInfo.upgradeName + "SpeedVal", speedVal);

        var stackVal = upgradeInfo.stackValues[upgradeLevel];
        upgradeInfo.workerStack.Value = stackVal;
        PlayerPrefs.SetInt(upgradeInfo.upgradeName + "StackVal", stackVal);

        SetLvlText(upgradeLevel);

        if (upgradeLevel + 1 < upgradeInfo.upgradePrices.Count)
        {
            SetPriceText(upgradeInfo.upgradePrices[upgradeLevel + 1]);
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
