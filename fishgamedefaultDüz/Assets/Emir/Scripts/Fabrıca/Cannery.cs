using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Cannery : MonoBehaviour
{
    [SerializeField] PlayerBag playerBag;
    [SerializeField] Slider slider;
    float timer;
    [SerializeField] Transform fishMoveStartPos;
    [SerializeField] Transform fishMoveProcessPos;
    List<CollectedFish> fishList;
    [SerializeField] CannSpawner cannSpawner;
    public int fishCount => lastIndex + 1;
    bool isCollecting;

    [SerializeField] int rowCount = 3;
    [SerializeField] int columnCount = 3;
    [SerializeField] Int fishCapacity;
    private int totalPositions;
    [SerializeField] Float processDurationPerStack;
    int lastIndex = -1;

    [SerializeField] GameObject warnText;
    [SerializeField] WorkerSecond secondWorker;

    public bool IsFull => fishCount == fishCapacity.Value;


    private void Awake()
    {
        totalPositions = rowCount * columnCount;
        fishList = new List<CollectedFish>();
        SetProcessTimerPercent(0f);
    }

    FishSO processFish;
    float processTimer;
    float targetDuration;
    bool isGettingFish;
    [SerializeField] GameObject maxTxt;

    void Update()
    {
        if (processFish == null)
        {
            if (isCollecting)
                return;

            if (fishList.Count == 0)
                return;

            if (!cannSpawner.CanSpawn())
                return;

            isGettingFish = true;
            int index = fishList.Count - 1;
            CollectedFish collectedFish = fishList[index];
            lastIndex--;
            if (maxTxt.activeSelf)
                maxTxt.SetActive(false);
            fishList.RemoveAt(index);
            collectedFish.transform.DOMove(fishMoveProcessPos.position, 0.2f).OnComplete(() => collectedFish.gameObject.SetActive(false));
            processFish = collectedFish.fishSO;
            targetDuration = processFish.capacity * processDurationPerStack.Value;
            isGettingFish = false;
        }
        else
        {
            processTimer += Time.deltaTime;
            SetProcessTimerPercent(processTimer / targetDuration);

            if (processTimer >= targetDuration)
            {
                int amount = processFish.capacity;
                SpawnCann(amount);

                processTimer = 0;
                SetProcessTimerPercent(0);

                processFish = null;
            }
        }
    }

    private void SetProcessTimerPercent(float percent)
    {
        slider.value = percent;
    }

    private void SpawnCann(int capacity)
    {
        cannSpawner.SpawnCan(capacity);
    }

    private Vector3 CalculateTargetPos()
    {
        lastIndex++;
        int layer = lastIndex / totalPositions; // Katman hesaplaması
        int tempLastIndex = lastIndex % totalPositions; // Pozisyon hesaplaması

        int rowIndex = tempLastIndex / columnCount;
        int columnIndex = tempLastIndex % columnCount;

        Vector3 targetPos = new Vector3(columnIndex * 1.6f, layer * 0.5f, rowIndex * 1.6f);
        return targetPos;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time < timer)
                return;

            if (isGettingFish)
                return;

            timer = Time.time + 0.1f;
            if (lastIndex + 1 >= fishCapacity.Value)
            {
                if (warnText.activeSelf)
                    warnText.SetActive(false);
                if (!maxTxt.activeSelf)
                    maxTxt.SetActive(true);
                StopCollect();
                return;
            }
            if (playerBag.IsCarryingFish())
            {
                if (playerBag.GetFishFromBag(out CollectedFish fish))
                {
                    isCollecting = true;
                    fish.transform.SetParent(fishMoveStartPos);
                    fish.transform.eulerAngles = Vector3.zero;
                    fish.transform.DOLocalJump(CalculateTargetPos(), 1f, 1, 0.2f);
                    fishList.Add(fish);
                }
                else
                {
                    StopCollect();
                }
            }
            else
            {
                if (!warnText.activeSelf)
                    warnText.SetActive(true);
                StopCollect();
            }
        }
        else if (other.CompareTag("Worker"))
        {
            if (Time.time < timer)
                return;

            if (isGettingFish)
                return;

            timer = Time.time + 0.1f;
            if (lastIndex + 1 >= fishCapacity.Value)
            {
                if (warnText.activeSelf)
                    warnText.SetActive(false);
                if (!maxTxt.activeSelf)
                    maxTxt.SetActive(true);
                StopCollect();
                return;
            }
            if (secondWorker.GetFishFromBag(out CollectedFish fish))
            {
                isCollecting = true;
                fish.transform.SetParent(fishMoveStartPos);
                fish.transform.eulerAngles = Vector3.zero;
                fish.transform.DOLocalJump(CalculateTargetPos(), 1f, 1, 0.2f);
                fishList.Add(fish);
            }
            else
            {
                StopCollect();
            }
        }
    }

    private void StopCollect()
    {
        if (!isCollecting)
            return;

        isCollecting = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCollect();
            if (warnText.activeSelf)
                warnText.SetActive(false);
        }
    }


}
