using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


public class WorkerBag : MonoBehaviour
{

    [SerializeField] private Int storageCapacity;
    [SerializeField] private Int storageCurrent;
    public bool IsFull => storageCurrent.Value == storageCapacity.Value;
    [SerializeField] TextMeshPro storageTMP;
    List<CollectedFish> FishInBagTemp;
    public int FishCount => FishInBagTemp.Count;
    private int lastIndex;
    [SerializeField] private float PickDistance = 1f;
    [SerializeField] Transform textsParent;


    public Transform target;

    void LateUpdate()
    {
        textsParent.position = transform.position;
    }

    private void Awake() {
        FishInBagTemp = new List<CollectedFish>();
        storageCurrent.Value = 0;
    }

    private void Start()
    {
        target.gameObject.SetActive(false);
        storageTMP.SetText(storageCurrent.Value + "/" + storageCapacity.Value);
    }

    public bool AddCatchFishToInventory(CollectedFish fish)
    {
        if (lastIndex == storageCapacity.Value)
        {
            return false;
        }
        lastIndex++;
        storageCurrent.Value++;
        storageTMP.SetText(storageCurrent.Value + "/" + storageCapacity.Value);
        fish.transform.SetParent(transform.GetChild(1));
        fish.transform.localEulerAngles = Vector3.zero;
        fish.transform.DOLocalJump(new Vector3(0, lastIndex * PickDistance, 0), 2, 1, 0.2f).OnComplete(() => { });
        FishInBagTemp.Add(fish);
        target.gameObject.SetActive(true);
        return true;
    }
    public bool GetFishFromBag(out CollectedFish fish)
    {
        if (lastIndex == 0)
        {
            fish = null;
            return false;
        }
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



}
