using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class SellFishArea : MonoBehaviour
{
    //Satış yaptğımız yer
    [SerializeField] private BoatBag boatBag;
    [SerializeField] private PlayerBag playerBag;
    [SerializeField] private WorkerBag workerBag;
    [SerializeField] WorkerSeller workerSeller;
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

    [SerializeField] WorkerSecond workerSecond;
    float playerTimer;
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isSelling)
            {
                return;
            }
            if (Time.time >= playerTimer)
            {
                playerTimer = Time.time + 0.1f;
                CollectedFish collectedFish;
                if (playerBag.GetFishFromBag(out collectedFish))
                {
                    isGettingFromPlayer = true;
                    if (boatBag.canPickupFish)
                    {
                        boatBag.AddFishToBoat(collectedFish);
                    }
                    else
                    {
                        collectedFish.transform.SetParent(transform);
                        collectedFish.transform.localEulerAngles = Vector3.zero;
                        CollectedFish localFish = collectedFish;
                        localFish.transform.DOLocalJump(CalculateTargetPos(), 3f, 1, animDuration).OnComplete(() =>
                            AddFishToList(localFish));

                    }
                    return;
                }
                else
                {
                    isGettingFromPlayer = false;
                    isPicking = false;
                }
            }
        }
        if (other.CompareTag("Worker"))
        {
            if (isSelling)
            {
                return;
            }

            if (Time.time >= timer)
            {
                if (other.gameObject == workerBag.gameObject)
                {
                    timer = Time.time + 0.1f;
                    if (workerBag.GetFishFromBag(out var collectedFish))
                    {
                        isGettingFromAi = true;
                        if (boatBag.canPickupFish)
                        {
                            boatBag.AddFishToBoat(collectedFish);
                        }
                        else
                        {
                            collectedFish.transform.SetParent(transform);
                            collectedFish.transform.localEulerAngles = Vector3.zero;
                            CollectedFish localFish = collectedFish;
                            localFish.transform.DOLocalJump(CalculateTargetPos(), 3f, 1, animDuration).OnComplete(() =>
                                AddFishToList(localFish));

                        }
                    }
                    else
                    {
                        isGettingFromAi = false;
                    }
                }
                else
                {
                    timer = Time.time + 0.1f;
                    if (workerSecond.GetFishFromBag(out var collectedFish))
                    {
                        isGettingFromAi = true;
                        if (boatBag.canPickupFish)
                        {
                            boatBag.AddFishToBoat(collectedFish);
                        }
                        else
                        {
                            collectedFish.transform.SetParent(transform);
                            collectedFish.transform.localEulerAngles = Vector3.zero;
                            CollectedFish localFish = collectedFish;
                            localFish.transform.DOLocalJump(CalculateTargetPos(), 3f, 1, animDuration).OnComplete(() =>
                                AddFishToList(localFish));

                        }
                    }
                    else
                    {
                        isGettingFromAi = false;
                    }
                }

            }
            //PickCatchedFishW();

            return;
        }

        if (other.CompareTag("WorkerSeller"))
        {
            if (isSelling)
            {
                return;
            }
            if (Time.time >= timer)
            {
                timer = Time.time + 0.1f;
                if (workerSeller.GetFishFromBag(out var collectedFish))
                {
                    isGettingFromAi = true;
                    if (boatBag.canPickupFish)
                    {
                        boatBag.AddFishToBoat(collectedFish);
                    }
                    else
                    {
                        collectedFish.transform.SetParent(transform);
                        collectedFish.transform.localEulerAngles = Vector3.zero;
                        CollectedFish localFish = collectedFish;
                        localFish.transform.DOLocalJump(CalculateTargetPos(), 3f, 1, animDuration).OnComplete(() =>
                            AddFishToList(localFish));

                    }
                }
                else
                {
                    isGettingFromAi = false;
                }
            }
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

        if (other.CompareTag("Worker"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;

            }
            isGettingFromAi = false;
            isPicking = true;
        }
    }
    public void PickCatchedFishP()
    {
        if (coroutine == null && isPicking && !isSelling)
        {
            coroutine = StartCoroutine(SellFishPlayer());
        }
    }
    public void PickCatchedFishW()
    {
        if (coroutine == null && isPicking)
        {
            coroutine = StartCoroutine(SellFishWorker());
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
            if (boatBag.canPickupFish)
            {
                boatBag.AddFishToBoat(collectedFish);
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
            boatBag.TryGetFishFromArea();
    }
    IEnumerator SellFishWorker()
    {
        WaitForSeconds timer = new WaitForSeconds(0.1f);
        CollectedFish collectedFish;
        while (workerBag.GetFishFromBag(out collectedFish))
        {
            if (boatBag.canPickupFish)
            {
                boatBag.AddFishToBoat(collectedFish);
            }
            else
            {
                collectedFish.transform.SetParent(transform);
                collectedFish.transform.localEulerAngles = Vector3.zero;

                CollectedFish localFish = collectedFish;
                localFish.transform.DOLocalJump(CalculateTargetPos(), 3f, 1, animDuration).OnComplete(
                    () => onGroundFishList.Add(localFish));

            }
            yield return timer;
        }
        isPicking = false;
    }
}
