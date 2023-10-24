using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.TextCore;
using TMPro;

public class FishPoolList
{
    public FishSO fishSO;
    public List<CollectedFish> collectedList;

    public FishPoolList()
    {
        collectedList = new List<CollectedFish>();
    }
}

public class CatchedFishArea : MonoBehaviour
{

    [SerializeField] GameObject uiParent;
    [SerializeField] public FishPoolSO fishPool;
    [SerializeField] HookCollector hookCollector;
    [SerializeField] TextMeshProUGUI capacityText;
    public List<FishPoolList> fishPoolLists;
    public List<CollectedFish> catchedFishs;
    public int fishCount => catchedFishs.Count;
    private Coroutine coroutine;
    private bool isPicking = true;
    [SerializeField] private PlayerBag playerBag;
    [SerializeField] private WorkerBag workerBag;
    [SerializeField] WorkerSecond workerSecond;
    public Transform startPos;
    public int rowCount = 3;
    public int columnCount = 3;
    private int lastIndex = -1;
    private int totalPositions;
    [SerializeField] Int maxStack;
    [SerializeField] GameObject stackIsFullTxt;
    public int CurrentAmount => lastIndex + 1;
    public int MaxStack => maxStack.Value;

    void Awake()
    {
        fishPoolLists = new List<FishPoolList>();
        FishPoolList fishPoolList;
        foreach (var poolFish in fishPool.poolFishList)
        {
            fishPoolList = new FishPoolList();
            for (int i = 0; i < poolFish.poolSize; i++)
            {
                var carryFish = Instantiate(poolFish.fishSO.carryObject);
                carryFish.transform.SetParent(transform);
                carryFish.SetActive(false);
                fishPoolList.collectedList.Add(carryFish.GetComponent<CollectedFish>());
            }
            fishPoolList.fishSO = fishPoolList.collectedList[0].fishSO;
            fishPoolLists.Add(fishPoolList);
        }

        catchedFishs = new List<CollectedFish>();

        totalPositions = rowCount * columnCount;

        maxStack.OnValueChangedEvent.AddListener(UpdateStackUI);
    }

    private void OnDisable() {
        maxStack.OnValueChangedEvent.RemoveListener(UpdateStackUI);
    }

    private void UpdateStackUI(int maxStackVal)
    {
        CheckIsFull();
    }

    private void Start() {
        CheckIsFull();
        uiParent.SetActive(true);
    }

    private void CheckIsFull()
    {
        capacityText.SetText((lastIndex + 1) + "/" + maxStack.Value);
        if (lastIndex < maxStack.Value - 1)
        {
            if (stackIsFullTxt.activeSelf)
            {
                stackIsFullTxt.SetActive(false);
                hookCollector.disableCollect = false;
            }
        }
        else if (!stackIsFullTxt.activeSelf)
        {
            stackIsFullTxt.SetActive(true);
            hookCollector.disableCollect = true;
        }
    }

    float timer;
    float workerTimer;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // PickCatchedFish();
            if (Time.time >= timer)
            {
                timer = Time.time + 0.1f;
                if (fishCount > 0)
                {
                    if (playerBag.AddCatchFishToInventory(catchedFishs[catchedFishs.Count - 1]))
                    {
                        catchedFishs.RemoveAt(catchedFishs.Count - 1);
                        lastIndex--;
                        CheckIsFull();
                    }
                    else
                    {
                        isPicking = false;
                    }
                }
                else
                {
                    isPicking = false;
                }

            }
            return;
        }
        if (other.CompareTag("Worker"))
        {
            if (Time.time >= workerTimer)
            {
                workerTimer = Time.time + 0.1f;
                if (fishCount > 0)
                {
                    if (workerBag == null)
                    {
                        if (workerSecond.AddCatchFishToInventory(catchedFishs[catchedFishs.Count - 1]))
                        {
                            catchedFishs.RemoveAt(catchedFishs.Count - 1);
                            lastIndex--;
                            CheckIsFull();
                        }
                    }
                    else
                    {
                        if (workerBag.AddCatchFishToInventory(catchedFishs[catchedFishs.Count - 1]))
                        {
                            catchedFishs.RemoveAt(catchedFishs.Count - 1);
                            lastIndex--;
                            CheckIsFull();
                        }
                    }

                }
            }
            //PickCatchedFishW();
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
        }
        if (other.CompareTag("Worker"))
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;

            }
            isPicking = true;
        }
    }
    public void AddFish(FishSO fishSO)
    {
        FishPoolList fishPoolList = fishPoolLists.Find(x => x.fishSO == fishSO);
        var deactiveFish = fishPoolList.collectedList.Find(x => !x.gameObject.activeSelf);
        if (deactiveFish != null && lastIndex < maxStack.Value - 1)
        {
            lastIndex++;
            CheckIsFull();
            deactiveFish.transform.SetParent(transform);
            deactiveFish.gameObject.SetActive(true);
            Vector3 targetPos = startPos.localPosition;
            int layer = lastIndex / totalPositions; // Katman hesaplaması
            int tempLastIndex = lastIndex % totalPositions; // Pozisyon hesaplaması

            int rowIndex = tempLastIndex / columnCount;
            int columnIndex = tempLastIndex % columnCount;

            targetPos = targetPos + new Vector3(columnIndex * 1.5f, layer * 1f, rowIndex * 1.5f);

            deactiveFish.transform.localPosition = targetPos;
            catchedFishs.Add(deactiveFish);
        }
    }


    public void AddFishFisher(FishSO fishSO, Transform fisher)
    {
        FishPoolList fishPoolList = fishPoolLists.Find(x => x.fishSO == fishSO);
        var deactiveFish = fishPoolList.collectedList.Find(x => !x.gameObject.activeSelf);
        if (deactiveFish != null && lastIndex < maxStack.Value - 1)
        {
            lastIndex++;
            CheckIsFull();
            deactiveFish.transform.position = fisher.position;
            deactiveFish.transform.SetParent(transform);
            deactiveFish.gameObject.SetActive(true);

            Vector3 targetPos = startPos.localPosition;
            int layer = lastIndex / totalPositions; // Katman hesaplaması
            int tempLastIndex = lastIndex % totalPositions; // Pozisyon hesaplaması
            int rowIndex = tempLastIndex / columnCount;
            int columnIndex = tempLastIndex % columnCount;
            targetPos = targetPos + new Vector3(columnIndex * 1.5f, layer * 1f, rowIndex * 1.5f);
            deactiveFish.transform.DOLocalJump(targetPos, 2f, 1, 0.2f).OnComplete(() =>
            {
                catchedFishs.Add(deactiveFish);
            });
        }
    }


    public void PickCatchedFish()
    {
        if (coroutine == null && isPicking)
        {
            coroutine = StartCoroutine(CatchFishPlayer());
        }
    }
    public void PickCatchedFishW()
    {
        if (coroutine == null && isPicking)
        {
            coroutine = StartCoroutine(CatchFishWorker());
        }
    }

    IEnumerator CatchFishPlayer()
    {
        WaitForSeconds timer = new WaitForSeconds(0.1f);

        while (catchedFishs.Count > 0)
        {
            if (playerBag.AddCatchFishToInventory(catchedFishs[catchedFishs.Count - 1]))
            {
                catchedFishs.RemoveAt(catchedFishs.Count - 1);
                lastIndex--;
                CheckIsFull();
            }
            else
            {
                isPicking = false;
                break;
            }
            yield return timer;

        }
        isPicking = false;
    }

    IEnumerator CatchFishWorker()
    {
        WaitForSeconds timer = new WaitForSeconds(0.1f);

        while (catchedFishs.Count > 0)
        {
            if (workerBag.AddCatchFishToInventory(catchedFishs[catchedFishs.Count - 1]))
            {
                catchedFishs.RemoveAt(catchedFishs.Count - 1);
                lastIndex--;
                CheckIsFull();
            }
            else
            {
                isPicking = false;
                break;
            }
            yield return timer;

        }
        isPicking = false;
    }
}
