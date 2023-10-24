using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReduceSomethingArea : TriggerAreas
{
    [SerializeField] TMP_Text priceText;
    [SerializeField] FishSO somethingType;
    [SerializeField] int neededSomething;
    [SerializeField] string areaName;
    [SerializeField] BuildingBase targetBulding;

    private bool isReducingGold;
    [SerializeField] float reductionRate = 1.0f; // Initial reduction rate in gold per second.

    void Awake()
    {
        if (PlayerPrefs.GetInt(areaName, 0) == 1)
        {
            targetBulding.Load();
            Destroy(gameObject);
        }
        else
        {
            var reaminingGold = PlayerPrefs.GetString(areaName + "value", "0");
            if (reaminingGold != "0")
            {
                neededSomething = int.Parse(reaminingGold);
            }
            priceText.SetText(neededSomething.ToString());
        }
    }

    protected override void OnPlayerEnter(Collider other)
    {
        base.OnPlayerEnter(other);

        // Start reducing gold if there is still gold needed.
        if (neededSomething > 0)
        {
            isReducingGold = true;
        }
    }

    protected override void OnPlayerExit(Collider other)
    {
        base.OnPlayerExit(other);

        // Stop reducing gold when the player exits.
        isReducingGold = false;
        PlayerPrefs.SetString(areaName + "value", neededSomething.ToString());
    }

    // Call this method to complete the payment when the player has paid all the needed gold.
    private void CompletePayment()
    {
        // AdManager.Instance.ShowAd();
        targetBulding.BuildAnim();
        PlayerPrefs.SetInt(areaName, 1);
        Destroy(gameObject);
    }

    float timer;

    private void OnTriggerStay(Collider other)
    {
        // Reduce gold over time if the reduction process is active.
        if (isReducingGold)
        {
            if (!PlayerBag.Instance.IsCarryingSameType(somethingType))
            {
                isReducingGold = false;
                return;
            }

            if (Time.time < timer)
                return;

            timer = Time.time + reductionRate;
            if (PlayerBag.Instance.GetFishFromBag(out var collectedFish))
            {
                // Deduct gold from the player's total.
                neededSomething--;
                collectedFish.transform.SetParent(null);
                collectedFish.gameObject.SetActive(false);

                AudioManager.Instance.PlayOpeningClipAt(transform.position);

                // Update the remaining gold needed.
 
                priceText.SetText(neededSomething.ToString());

                // Check if the payment is complete.
                if (neededSomething <= 0)
                {
                    // Payment is complete, stop reducing gold and call the completion method.
                    isReducingGold = false;
                    CompletePayment();
                }
            }
        }
    }

}
