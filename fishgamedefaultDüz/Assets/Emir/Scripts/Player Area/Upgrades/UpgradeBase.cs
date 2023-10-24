using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public abstract class UpgradeBase : MonoBehaviour
{

    public abstract bool IsUpgradeMax { get; }
    public TextMeshProUGUI lvlText;
    public TextMeshProUGUI maxLvlText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI upgradeAmountText;
    public GameObject btns;
    public GameObject maxTMP;
    public Action OnUpgrade;
    public abstract long currentPrice { get; }
    [SerializeField] private Button upButton;


    protected virtual void OnDestroy()
    {
        if (PlayerManager.Instance)
            PlayerManager.Instance.gold.OnValueChangedEvent.RemoveListener(CheckGold);
    }
    protected virtual void OnEnable()
    {
        PlayerManager.Instance.gold.OnValueChangedEvent.AddListener(CheckGold);
    }
    private void CheckGold(long gold)
    {
        if (gold < currentPrice)
        {
            upButton.interactable = false;
        }
        else
        {
            upButton.interactable = true;
        }
    }
    protected void ShowMaxTMP()
    {
        btns.SetActive(false);
        maxTMP.SetActive(true);
        maxLvlText.gameObject.SetActive(false);
    }
    public void SetLvlText(int lvl)
    {
        lvlText.SetText("LVL " + lvl);
    }
    public void SetPriceText(long price)
    {
        CheckGold(PlayerManager.Instance.gold.Value);
        priceText.SetText(price.ToString());
    }
    public void SetMaxLvlText(int maxLvl)
    {
        maxLvlText.SetText("MAX LVL " + (maxLvl - 1));
    }
    public void SetUpgradeInfoText(int cnt, int lvl, int upLwl)
    {
        upgradeText.SetText(lvl + " to " + upLwl);
        upgradeAmountText.SetText(" + " + cnt);
    }

    public void SetUpgradeInfoText(float cnt, float lvl, float upLwl)
    {
        string cntStr = cnt == ((int)cnt) ? ((int)cnt).ToString() : cnt.ToString("0.0");
        string lvlStr = lvl == ((int)lvl) ? ((int)lvl).ToString() : lvl.ToString("0.0");
        string upLwlStr = upLwl == ((int)upLwl) ? ((int)upLwl).ToString() : upLwl.ToString("0.0");
        upgradeText.SetText(lvlStr + " to " + upLwlStr);
        upgradeAmountText.SetText(" + " + cntStr);
    }
}
