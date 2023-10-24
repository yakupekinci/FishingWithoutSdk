using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using DG.Tweening;

public class FabricaBag : MonoBehaviour
{

    [SerializeField] private FabricaArea fabricaArea;
    [SerializeField] public Int storageCapacity;
    [SerializeField] public Int storageCurrent;
    [SerializeField] public TextMeshPro storageTMP;
    public CollectConserveArea collectGoldArea;
    List<CollectedFish> fishInBoat;
    public bool canPickupFish = true;
    bool isPicking = false;
    float timer;
    bool waitConserve = true;

    void Start()
    {
        fishInBoat = new List<CollectedFish>();
        storageCurrent.Value = 0;
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
            while (fabricaArea.TryGetFish(out collectedFish))
            {
                fabricaArea.isSelling = true;
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
        fabricaArea.isSelling = false;
        isPicking = false;
    }

    public void ResetCount()
    {
        storageCurrent.Value = 0;
        storageTMP.text = storageCurrent.Value + "/" + storageCapacity.Value;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(2f);
        waitConserve = true;
    }

    public bool Leave()
    {



        return waitConserve;
    }
    private void StopTakeFish()
    {
        if (storageCurrent.Value == storageCapacity.Value)
        {

            if (Leave())
            {
                foreach (var conserve in fishInBoat)
                {
                    Debug.Log("girdi");
                    collectGoldArea.AddMoney(conserve.fishSO.gold);
                    fishInBoat.Clear();
                }

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
