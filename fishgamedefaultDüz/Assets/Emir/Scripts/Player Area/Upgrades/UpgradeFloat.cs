using UnityEngine;

public class UpgradeFloat : UpgradeBase
{
    public override long currentPrice => upgradeInfo.upgradePrices[Mathf.Min(upgradeLevel + 1, upgradeInfo.upgradePrices.Count - 1)];
    public FloatUpgradeSO upgradeInfo;
    int upgradeLevel;
    bool isLoaded;
    float diff;
    float currentValue;
    float nextValue;

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
            float diffa = upgradeInfo.upgradeValues[upgradeLevel + 1] - upgradeInfo.upgradeValues[upgradeLevel];
            float currentValuea = upgradeInfo.upgradeValues[upgradeLevel];
            float nextValuea = upgradeInfo.upgradeValues[upgradeLevel + 1];
            diff = diffa;
            currentValue = currentValuea;
            nextValue = nextValuea;
            SetUpgradeInfoText(diff, currentValue, nextValue);
        }

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
        /* AdManager.Instance.ShowAd(Upgrade, null); */
        Upgrade();
    }

    private void Upgrade()
    {
        upgradeLevel++;
        PlayerPrefs.SetInt(upgradeInfo.upgradeName, upgradeLevel);
        float value = upgradeInfo.upgradeValues[upgradeLevel];
        upgradeInfo.upgradeFloat.Value = value;
        PlayerPrefs.SetFloat(upgradeInfo.upgradeName + "val", value);
        SetLvlText(upgradeLevel);

        if (upgradeLevel + 1 < upgradeInfo.upgradePrices.Count)
        {

            SetPriceText(upgradeInfo.upgradePrices[upgradeLevel + 1]);

            float diffa = upgradeInfo.upgradeValues[upgradeLevel + 1] - upgradeInfo.upgradeValues[upgradeLevel];
            float currentValuea = upgradeInfo.upgradeValues[upgradeLevel];
            float nextValuea = upgradeInfo.upgradeValues[upgradeLevel + 1];
            diff = diffa;
            currentValue = currentValuea;
            nextValue = nextValuea;
            SetUpgradeInfoText(diff, currentValue, nextValue);
            //  Debug.Log(upgradeLevel);


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
