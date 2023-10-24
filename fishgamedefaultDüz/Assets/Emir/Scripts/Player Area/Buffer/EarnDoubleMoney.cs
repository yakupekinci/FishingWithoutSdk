using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarnDoubleMoney : TriggerAreas
{
    [SerializeField] DoubleMoneySpawner doubleMoneySpawner;
    [SerializeField] GameObject happyTimeAskUi;

    [SerializeField] float happyTime = 60f;
    bool isActive;

    protected override void OnPlayerEnter(Collider other)
    {
        base.OnPlayerEnter(other);
        if (other.CompareTag("Player"))
        {
            if (isActive)
                return;

            isActive = true;
            happyTimeAskUi.SetActive(true);
            PlayerMovement.Instance.DisableMovement();
            InGameUI.Instance.ActiveUI = happyTimeAskUi;
        }
    }

    public void OnClicked()
    {
        if (isActive)
            return;

        isActive = true;
        happyTimeAskUi.SetActive(true);
        PlayerMovement.Instance.DisableMovement();
        InGameUI.Instance.ActiveUI = happyTimeAskUi;
    }

    protected override void OnPlayerExit(Collider other)
    {
        base.OnPlayerExit(other);
        isActive = false;
    }

    private void CallDoubleMoney()
    {
        transform.parent.gameObject.SetActive(false);
        AdManager.Instance.ShowHappyTime();
        doubleMoneySpawner.Earned();
        PlayerMovement.Instance.EnableMovement();
        PlayerManager.Instance.EarnMoreMoney(happyTime);
        isActive = false;
    }

    public void OnCancelPressed()
    {
        happyTimeAskUi.SetActive(false);
        isActive = false;
        AudioManager.Instance.PlayUISoundAtGameCam();
        PlayerMovement.Instance.EnableMovement();
    }

    public void OnWatchPressed()
    {
        happyTimeAskUi.SetActive(false);
        AudioManager.Instance.PlayUISoundAtGameCam();
        /*
        AdManager.Instance.ShowAd(CallDoubleMoney, ErrorAd);
        */
        doubleMoneySpawner.OpenCloseMoney();
        CallDoubleMoney();
    }

    private void ErrorAd()
    {
        isActive = false;
        PlayerMovement.Instance.EnableMovement();
        doubleMoneySpawner.Earned();
    }

}
