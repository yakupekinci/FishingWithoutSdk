using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CannSpawner : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI capacityTxt;
    [SerializeField] Transform canStartPos;
    [SerializeField] int rowCount = 3;
    [SerializeField] int columnCount = 3;
    private int totalPositions;
    int lastIndex = -1;
    [SerializeField] GameObject canPrefab;
    List<CollectedFish> cannPool;
    List<CollectedFish> activeCannList;
    [SerializeField] public Int cannCapacity;
    [SerializeField] GameObject maxTxt;
    [SerializeField] GameObject UI;

    public int CannCount => lastIndex + 1;

    void Awake()
    {
        totalPositions = rowCount * columnCount;

        cannPool = new List<CollectedFish>();
        activeCannList = new List<CollectedFish>();
        canPrefab.SetActive(false);

        for (int i = 0; i < 100; i++)
        {
            cannPool.Add(Instantiate(canPrefab, Vector3.zero, Quaternion.identity).GetComponent<CollectedFish>());
        }
    }
    void Start()
    {
        
        UI.SetActive(true);
        capacityTxt.SetText("0/" + cannCapacity.Value);
        cannCapacity.OnValueChangedEvent.AddListener(UpdateCapacityUI);
    }

    private void OnDisable() {
        cannCapacity.OnValueChangedEvent.RemoveListener(UpdateCapacityUI);
    }

    private void UpdateCapacityUI(int val)
    {
        capacityTxt.SetText(CannCount.ToString() + "/" + cannCapacity.Value);
    }

    public bool CanSpawn()
    {
        if (lastIndex + 1 < cannCapacity.Value)
            return true;

        if (!maxTxt.activeSelf)
            maxTxt.SetActive(true);
        return false;
    }

    public void SpawnCan(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var cann = cannPool.Find(x => !x.gameObject.activeSelf);
            if (cann)
            {
                cann.transform.SetParent(canStartPos);
                cann.transform.localPosition = CalculateTargetPos();
                cann.gameObject.SetActive(true);
                activeCannList.Add(cann);
            }
            capacityTxt.SetText(CannCount.ToString() + "/" + cannCapacity.Value);
        }
    }

    private Vector3 CalculateTargetPos()
    {
        lastIndex++;
        int layer = lastIndex / totalPositions; // Katman hesaplaması
        int tempLastIndex = lastIndex % totalPositions; // Pozisyon hesaplaması

        int rowIndex = tempLastIndex / columnCount;
        int columnIndex = tempLastIndex % columnCount;

        Vector3 targetPos = new Vector3(columnIndex * 1.5f, layer * 0.5f, rowIndex * 1.6f);
        return targetPos;
    }

    [SerializeField] PlayerBag playerBag;
    [SerializeField] WorkerSeller workerSeller;
    float timer;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time < timer)
                return;


            timer = Time.time + 0.1f;
            if (activeCannList.Count == 0)
                return;

            int index = activeCannList.Count - 1;
            var cann = activeCannList[activeCannList.Count - 1];
            if (playerBag.AddCannToInventory(cann))
            {
                activeCannList.RemoveAt(index);
                lastIndex--;
                capacityTxt.SetText(CannCount.ToString() + "/" + cannCapacity.Value);
                if (maxTxt.activeSelf)
                    maxTxt.SetActive(false);
            }
        }
        else if (other.CompareTag("WorkerSeller"))
        {
            if (Time.time < timer)
                return;


            timer = Time.time + 0.1f;
            if (activeCannList.Count == 0)
                return;

            int index = activeCannList.Count - 1;
            var cann = activeCannList[activeCannList.Count - 1];
            if (workerSeller.AddCatchFishToInventory(cann))
            {
                activeCannList.RemoveAt(index);
                lastIndex--;
                capacityTxt.SetText(CannCount.ToString() + "/" + cannCapacity.Value);
                if (maxTxt.activeSelf)
                    maxTxt.SetActive(false);
            }
        }
    }

}
