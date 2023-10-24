using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using DG.Tweening;

public class BoatBag : MonoBehaviour
{
    [SerializeField] private BoatMovement boatMovement;
    [SerializeField] private SellFishArea sellFishArea;
    [SerializeField] public Int storageCapacity;
    [SerializeField] public Int storageCurrent;
    [SerializeField] public TextMeshPro storageTMP;
    List<CollectedFish> fishInBoat;
    public bool canPickupFish = true;
    bool isPicking = false;
    float timer;

    void Start()
    {
        fishInBoat = new List<CollectedFish>();
        storageCurrent.Value = 0;
        storageTMP.text = storageCurrent.Value + "/" + storageCapacity.Value;
    }


    public void AddFishToBoat(CollectedFish collectedFish)
    {
        AddToStorage(collectedFish);
        if (storageCurrent.Value == storageCapacity.Value)
        {
            isPicking = false;
            canPickupFish = false;
        }

        collectedFish.transform.SetParent(transform);
        collectedFish.transform.localEulerAngles = Vector3.zero;
        CollectedFish localFish = collectedFish;
        localFish.transform.DOLocalJump(new Vector3(0, 0.5f, 0), 3f, 1, 0.1f).OnComplete(() =>
          {
              localFish.transform.gameObject.SetActive(false);
              StopTakeFish();
          });
    }

    public void TryGetFishFromArea()
    {
        if (!isPicking && canPickupFish)
        {
            StartCoroutine(StartGet());
        }
    }

    public void Reset()
    {
        canPickupFish = true;
        isPicking = false;
    }

    public void AfterLand()
    {
        Reset();
        TryGetFishFromArea();
    }

    void Update()
    {
        if (isPicking)
            return;

        if (Time.time >= timer)
        {
            TryGetFishFromArea();
            timer = Time.time + 2f;
        }
    }

    IEnumerator StartGet()
    {
        WaitForSeconds timer = new WaitForSeconds(0.2f);
        CollectedFish collectedFish;
        if (canPickupFish)
        {
            while (sellFishArea.TryGetFish(out collectedFish))
            {
                sellFishArea.isSelling = true;
                isPicking = true;
                AddToStorage(collectedFish);
                if (storageCurrent.Value == storageCapacity.Value)
                {
                    isPicking = false;
                    canPickupFish = false;
                }
                collectedFish.transform.SetParent(transform);
                collectedFish.transform.localEulerAngles = Vector3.zero;
                CollectedFish localFish = collectedFish;
                localFish.transform.DOLocalJump(new Vector3(0, 0.5f, 0), 3f, 1, 0.1f).OnComplete(() =>
                  {
                      localFish.transform.gameObject.SetActive(false);
                      StopTakeFish();
                  });
                yield return timer;
                if (!canPickupFish)
                {
                    break;
                }
            }
        }
        sellFishArea.isSelling = false;
        isPicking = false;
    }

    public void ResetCount()
    {
        storageCurrent.Value = 0;
        storageTMP.text = storageCurrent.Value + "/" + storageCapacity.Value;
    }

    public CollectGoldArea collectGoldArea;

    private void StopTakeFish()
    {
        if (storageCurrent.Value == storageCapacity.Value)
        {
            if (boatMovement.Leave())
            {
                long totalAmount = 0;
                foreach (var fish in fishInBoat)
                {
                    totalAmount += fish.fishSO.gold;
                    collectGoldArea.AddMoney(fish.fishSO.gold);
                }
                collectGoldArea.AddTotal(totalAmount);
                fishInBoat.Clear();
            }
        }
    }

    private void AddToStorage(CollectedFish collectedFish)
    {
        storageCurrent.Value += 1;
        fishInBoat.Add(collectedFish);
        storageTMP.text = storageCurrent.Value + "/" + storageCapacity.Value;
    }
   
}
