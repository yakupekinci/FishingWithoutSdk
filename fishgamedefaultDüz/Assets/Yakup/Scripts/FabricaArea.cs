using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class FabricaArea : MonoBehaviour
{
    //Satış yaptğımız yer
    [SerializeField] private FabricaBag fabricaBag;
    [SerializeField] private PlayerBag playerBag;

    [SerializeField] Transform startPos;
    List<CollectedFish> onGroundFishList;
    [SerializeField] private float animDuration = 1f;
    Coroutine coroutine;
    bool isPicking = true;
    public int rowCount = 3;
    public int columnCount = 3;
    private int totalPositions;
    public int lastIndex = -1;
    bool isGettingFromPlayer;
    bool isGettingFromAi;
    bool IsGettingFromOutside => isGettingFromAi || isGettingFromPlayer;
    public bool isSelling;
    float timer;

    private void Awake()
    {
        onGroundFishList = new List<CollectedFish>();
        totalPositions = rowCount * columnCount;

    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PickCatchedFishP();
            return;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;

            }
            isPicking = true;
            isGettingFromPlayer = false;
            return;
        }


    }
    public void PickCatchedFishP()
    {
        if (coroutine == null && isPicking && !isSelling)
        {
            coroutine = StartCoroutine(SellFishPlayer());
        }
    }



    public bool TryGetFish(out CollectedFish collectedFish)
    {
        if (onGroundFishList.Count <= 0)
        {
            collectedFish = null;
            return false;
        }

        if (IsGettingFromOutside)
        {
            collectedFish = null;
            return false;
        }

        collectedFish = onGroundFishList[onGroundFishList.Count - 1];
        onGroundFishList.RemoveAt(onGroundFishList.Count - 1);
        lastIndex--;
        return true;
    }

    IEnumerator SellFishPlayer()
    {
        WaitForSeconds timer = new WaitForSeconds(0.1f);
        CollectedFish collectedFish;
        while (playerBag.GetFishFromBag(out collectedFish))
        {
            isGettingFromPlayer = true;
            if (fabricaBag.canPickupFish)
            {
                fabricaBag.AddFishToBoat(collectedFish);
            }
            else
            {
                collectedFish.transform.SetParent(transform);
                collectedFish.transform.localEulerAngles = Vector3.zero;
                CollectedFish localFish = collectedFish;
                localFish.transform.DOLocalJump(CalculateTargetPos(), 3f, 1, animDuration).OnComplete(() =>
                 AddFishToList(localFish));

            }
            yield return timer;
        }
        isGettingFromPlayer = false;
        isPicking = false;
    }

    private Vector3 CalculateTargetPos()
    {
        lastIndex++;
        Vector3 targetPos = startPos.localPosition;
        int layer = lastIndex / totalPositions; // Katman hesaplaması
        int tempLastIndex = lastIndex % totalPositions; // Pozisyon hesaplaması

        int rowIndex = tempLastIndex / columnCount;
        int columnIndex = tempLastIndex % columnCount;

        targetPos = targetPos + new Vector3(columnIndex * 1.7f, layer * 0.5f, rowIndex * 1.5f);
        return targetPos;
    }

    private void AddFishToList(CollectedFish localFish)
    {
        onGroundFishList.Add(localFish);
        if (!IsGettingFromOutside)
            fabricaBag.TryGetFishFromArea();
    }
}
