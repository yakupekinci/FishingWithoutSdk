using UnityEngine;
using TMPro;

public class ReduceGoldArea : TriggerAreas
{
    public TMP_Text priceText;
    public long neededGold;
    public string areaName;
    public BuildingBase targetBulding;
    [SerializeField] GameObject openSecondObject;
    [SerializeField] GameObject closeShadosObject;
    private bool isReducingGold;
    public float reductionRate = 1.0f; // Initial reduction rate in gold per second.
    public int minGoldToReduce = 1;
    void Awake()
    {
        if (PlayerPrefs.GetInt(areaName, 0) == 1)
        {
            closeShadosObject.SetActive(false);
            targetBulding.Load();
            gameObject.SetActive(false);
            /*             if (openSecondObject)
                            openSecondObject.SetActive(true); */
        }
        else
        {
            targetBulding.gameObject.SetActive(false);
            /*  if (openSecondObject)
                 openSecondObject.SetActive(true); */
            var reaminingGold = PlayerPrefs.GetString(areaName + "value", "0");
            if (reaminingGold != "0")
            {
                neededGold = long.Parse(reaminingGold);
            }
            priceText.SetText(neededGold.ToString());
        }
    }
    protected override void OnPlayerEnter(Collider other)
    {
        base.OnPlayerEnter(other);

        // Start reducing gold if there is still gold needed.
        if (neededGold > 0)
        {
            isReducingGold = true;
        }
    }

    protected override void OnPlayerExit(Collider other)
    {
        base.OnPlayerExit(other);

        // Stop reducing gold when the player exits.
        isReducingGold = false;
        PlayerPrefs.SetString(areaName + "value", neededGold.ToString());
    }

    // Call this method to complete the payment when the player has paid all the needed gold.
    private void CompletePayment()
    {
        targetBulding.BuildAnim();
        closeShadosObject.SetActive(false);
        PlayerPrefs.SetInt(areaName, 1);
        gameObject.SetActive(false);
        if (openSecondObject)
        {
            if (openSecondObject.TryGetComponent<UnlockControl>(out var control))
            {
                control.OpenAndUnlock();
            }
        }
    }
    public bool isAdWatched = false;

    private void SkipPayment()
    {
        PlayerManager.Instance.ReduceGold((long)neededGold);
        isReducingGold = false;
        CompletePayment();
    }

    private void OnTriggerStay(Collider other)
    {
        // Reduce gold over time if the reduction process is active.
        if (isReducingGold)
        {
            long playerGold = PlayerManager.Instance.gold.Value;

            if (playerGold >= neededGold && !isAdWatched)
            {

                /*   isAdWatched = true;
                  AdManager.Instance.ShowMidRoll(SkipPayment, SkipPayment); */
              /*   isReducingGold = false;
                CompletePayment();
                return; */
            }
            // Calculate the gold that can be deducted in this frame.
            float goldToDeductThisFrame = reductionRate * Time.deltaTime;

            goldToDeductThisFrame = Mathf.Clamp(goldToDeductThisFrame, minGoldToReduce, playerGold);

            if (playerGold >= goldToDeductThisFrame && goldToDeductThisFrame != 0)
            {
                // Deduct gold from the player's total.
                PlayerManager.Instance.ReduceGold((long)goldToDeductThisFrame);

                AudioManager.Instance.PlayOpeningClipAt(transform.position);

                // Update the remaining gold needed.
                neededGold -= (long)goldToDeductThisFrame;
                priceText.SetText(neededGold.ToString());

                // Check if the payment is complete.
                if (neededGold <= 0)
                {
                    // Payment is complete, stop reducing gold and call the completion method.
                    isReducingGold = false;
                    CompletePayment();
                }
            }
        }
    }

}
