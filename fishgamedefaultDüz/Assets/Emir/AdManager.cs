using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class AdManager : Singleton<AdManager>
{
    [SerializeField] GameObject adLoadingPanel;
   /*  CrazyAds.AdBreakCompletedCallback completedCallbackP;
    CrazyAds.AdErrorCallback errorCallbackP; */
/*     CrazySDK.EventCallback OnStart;
    CrazySDK.EventCallback OnEnd; */

    protected override void Awake()
    {
        base.Awake();
      /*   CrazySDK.Instance.AddEventListener(CrazySDKEvent.adStarted, ev => StartAd());
        CrazySDK.Instance.AddEventListener(CrazySDKEvent.adblockDetectionExecuted, ev => EndAd());
        CrazySDK.Instance.AddEventListener(CrazySDKEvent.adError, ev => EndAd());
        CrazySDK.Instance.AddEventListener(CrazySDKEvent.adFinished, ev => EndAd());
        CrazySDK.Instance.AddEventListener(CrazySDKEvent.adCompleted, ev => EndAd()); */
    }

    private void OnDisable()
    {
       /*  if (CrazySDK.Instance)
        {
            CrazySDK.Instance.RemoveEventListener(CrazySDKEvent.adStarted, ev => StartAd());
            CrazySDK.Instance.RemoveEventListener(CrazySDKEvent.adblockDetectionExecuted, ev => EndAd());
            CrazySDK.Instance.RemoveEventListener(CrazySDKEvent.adError, ev => EndAd());
            CrazySDK.Instance.RemoveEventListener(CrazySDKEvent.adFinished, ev => EndAd());
            CrazySDK.Instance.RemoveEventListener(CrazySDKEvent.adCompleted, ev => EndAd());
        } */
    }

    public void TapToPlay()
    {
       /*  CrazySDK.Instance.GameplayStart(); */
    }

    public void OnPause()
    {
      /*   CrazySDK.Instance.GameplayStop(); */
    }
/* 
    public void ShowAd(CrazyAds.AdBreakCompletedCallback completedCallback, CrazyAds.AdErrorCallback errorCallback)
    {
        completedCallbackP = completedCallback;
        errorCallbackP = errorCallback;
        OnCall();
        CrazyAds.Instance.beginAdBreakRewarded(OnSuccess, OnError);
    }
 */
   /*  public void ShowMidRoll(CrazyAds.AdBreakCompletedCallback completedCallback, CrazyAds.AdErrorCallback errorCallback)
    {
        completedCallbackP = completedCallback;
        errorCallbackP = errorCallback;
        OnCall();
        CrazyAds.Instance.beginAdBreak(OnSuccess, OnError);
    } */

    public void ShowHappyTime()
    {
      /*   CrazyEvents.Instance.HappyTime(); */
    }

    private void OnSuccess()
    {
       /*  completedCallbackP?.Invoke(); */
        EndAd();
    }

    private void OnError()
    {
       /*  errorCallbackP?.Invoke(); */
        EndAd();
    }

    private void EndAd()
    {
        PlayerMovement.Instance.EnableMovement();
        EnableSounds();
        adLoadingPanel.SetActive(false);
    }

    private void OnCall()
    {
        PlayerMovement.Instance.DisableMovement();
        adLoadingPanel.SetActive(true);
    }

    private void StartAd()
    {
        DisableSounds();
    }

    private void DisableSounds()
    {
        AudioListener.volume = 0f;
    }

    private void EnableSounds()
    {
        AudioListener.volume = 1f;
    }

}
