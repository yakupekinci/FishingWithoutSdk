using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class PlayerBag : Singleton<PlayerBag>
{
    [SerializeField] FishSO[] notFishSO;
    [SerializeField] private Int storageCapacity;
    [SerializeField] private Int storageCurrent;
    [SerializeField] Transform bagTransform;
    [SerializeField] TextMeshPro storageTMP;
 
    List<CollectedFish> FishInBagTemp;

    private int lastIndex;
    [SerializeField] private float PickDistance = 1f;


    public Transform target;

    void LateUpdate()
    {
        target.position = transform.position;
    }

    private void Start()
    {
        target.gameObject.SetActive(false);
        storageCurrent.Value = 0;
        FishInBagTemp = new List<CollectedFish>();
        storageTMP.SetText(storageCurrent.Value + "/" + storageCapacity.Value);
    }

    public bool AddCatchFishToInventory(CollectedFish fish)
    {
        if (lastIndex == storageCapacity.Value)
        {
            return false;
        }
        if (lastIndex != 0)
        {
            if (!IsCarryingFish())
                return false;
        }
        AudioManager.Instance.PlayCollectBagAtPoint(transform.position);
        lastIndex++;
        storageCurrent.Value++;
        storageTMP.SetText(storageCurrent.Value + "/" + storageCapacity.Value);
        fish.transform.SetParent(bagTransform);
        fish.transform.localEulerAngles = Vector3.zero;
        fish.transform.DOLocalJump(new Vector3(0, lastIndex * PickDistance, 0), 2, 1, 0.2f).OnComplete(() => { });
        FishInBagTemp.Add(fish);
        target.gameObject.SetActive(true);
        return true;
    }

    public bool AddCannToInventory(CollectedFish fish)
    {
        if (lastIndex == storageCapacity.Value)
        {
            return false;
        }
        if (lastIndex != 0)
        {
            var inBag = FishInBagTemp[lastIndex - 1];
            if (inBag.fishSO != fish.fishSO)
            {
                fish = null;
                return false;
            }
        }
        AudioManager.Instance.PlayCollectBagAtPoint(transform.position);
        lastIndex++;
        storageCurrent.Value++;
        storageTMP.SetText(storageCurrent.Value + "/" + storageCapacity.Value);
        fish.transform.SetParent(bagTransform);
        fish.transform.localEulerAngles = Vector3.zero;
        fish.transform.DOLocalJump(new Vector3(0, lastIndex * PickDistance, 0), 2, 1, 0.2f).OnComplete(() => { });
        FishInBagTemp.Add(fish);
        target.gameObject.SetActive(true);
        return true;
    }

    public bool IsCarryingSameType(FishSO fishSO)
    {
        if (lastIndex == 0)
            return true;

        var fish = FishInBagTemp[lastIndex - 1];
        if (fishSO == fish.fishSO)
        {
            return true;
        }
        return false;
    }

    public bool GetFishFromBag(out CollectedFish fish)
    {
        if (lastIndex == 0)
        {

            fish = null;
            return false;
        }
        AudioManager.Instance.PlayReleaseBagAtPoint(transform.position);
        fish = FishInBagTemp[lastIndex - 1];
        FishInBagTemp.RemoveAt(lastIndex - 1);
        lastIndex--;
        storageCurrent.Value--;
        storageTMP.SetText(storageCurrent.Value + "/" + storageCapacity.Value);
        if (storageCurrent.Value == 0)
        {
            target.gameObject.SetActive(false);
        }
        return true;
    }

    public bool IsCarryingFish()
    {
        if (lastIndex == 0)
            return false;

        var fish = FishInBagTemp[lastIndex - 1];
        foreach (var notFish in notFishSO)
        {
            if (notFish == fish.fishSO)
            {
                fish = null;
                return false;
            }
        }
        return true;
    }

}
