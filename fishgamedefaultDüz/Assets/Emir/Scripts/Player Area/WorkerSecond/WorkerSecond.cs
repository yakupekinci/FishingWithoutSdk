using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using DG.Tweening;

public class WorkerSecond : MonoBehaviour
{
    enum WorkerSecondEnum
    {
        GoingToPickArea,
        Picking,
        GoingToCannArea,
        ReleasingFish,
        GoingToSellArea,
        Selling
    }


    [SerializeField] TextMeshPro capacityText;
    [SerializeField] GameObject needFishText;
    [SerializeField] Transform textParent;
    [SerializeField] Int maxCapacity;
    [SerializeField] float bagSpacing = 0.3f;
    NavMeshAgent agent;
    [SerializeField] Animator animator;
    List<CollectedFish> fishInBagTemp;
    [SerializeField] Float moveSpeed;
    int currentStack;

    [SerializeField] Transform pickAreaPosition;
    [SerializeField] CatchedFishArea catchedFishArea;
    [SerializeField] Cannery cannery;
    [SerializeField] Transform cannAreaPosition;
    [SerializeField] Transform sellAreaPosition;
    WorkerSecondEnum currentState;


    void Awake()
    {
        SetCapacity(0);
        agent = GetComponent<NavMeshAgent>();
        currentState = WorkerSecondEnum.GoingToPickArea;
        fishInBagTemp = new List<CollectedFish>();
    }

    private void Start()
    {
        PlayerPrefs.SetInt("workerSecondUnlocked", 1);

        agent.speed = moveSpeed.Value;

        moveSpeed.OnValueChangedEvent.AddListener(ChangeSpeed);

        animator.SetBool("isRunning", true);
    }

    private void OnDestroy()
    {
        moveSpeed.OnValueChangedEvent.RemoveListener(ChangeSpeed);
    }

    private void ChangeSpeed(float value)
    {
        agent.speed = value;
    }
    void Update()
    {
        if (currentState == WorkerSecondEnum.GoingToPickArea)
        {
            float distace = Vector3.Distance(transform.position, pickAreaPosition.position);
            if (distace <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                currentState = WorkerSecondEnum.Picking;
                animator.SetBool("isRunning", false);
            }
            else
            {
                agent.SetDestination(pickAreaPosition.position);
            }
            return;
        }

        if (currentState == WorkerSecondEnum.Picking)
        {
            if (currentStack == maxCapacity.Value)
            {
                needFishText.SetActive(false);
                agent.isStopped = false;
                currentState = WorkerSecondEnum.GoingToCannArea;
                animator.SetBool("isRunning", true);
            }
            else if (catchedFishArea.fishCount == 0)
            {
                if (currentStack > 0)
                {
                    needFishText.SetActive(false);
                    agent.isStopped = false;
                    currentState = WorkerSecondEnum.GoingToCannArea;
                    animator.SetBool("isRunning", true);
                }
                else
                {
                    needFishText.SetActive(true);
                }
            }
            else
            {
                // COLLECT LOGIC
            }
            return;
        }

        if (currentState == WorkerSecondEnum.GoingToCannArea)
        {
            float distace = Vector3.Distance(transform.position, cannAreaPosition.position);
            if (distace <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                currentState = WorkerSecondEnum.ReleasingFish;
                animator.SetBool("isRunning", false);
            }
            else
            {
                agent.SetDestination(cannAreaPosition.position);
            }
            return;
        }

        if (currentState == WorkerSecondEnum.ReleasingFish)
        {
            if (currentStack == 0)
            {
                agent.isStopped = false;
                currentState = WorkerSecondEnum.GoingToPickArea;
                animator.SetBool("isRunning", true);
            }
            else if (cannery.IsFull)
            {
                agent.isStopped = false;
                currentState = WorkerSecondEnum.GoingToSellArea;
                animator.SetBool("isRunning", true);
            }
            return;
        }

        if (currentState == WorkerSecondEnum.GoingToSellArea)
        {
            float distace = Vector3.Distance(transform.position, sellAreaPosition.position);
            if (distace <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                currentState = WorkerSecondEnum.Selling;
                animator.SetBool("isRunning", false);
            }
            else
            {
                agent.SetDestination(sellAreaPosition.position);
            }
            return;
        }

        if (currentState == WorkerSecondEnum.Selling)
        {
            if (currentStack == 0)
            {
                agent.isStopped = false;
                currentState = WorkerSecondEnum.GoingToPickArea;
                animator.SetBool("isRunning", true);
            }
            else
            {
                
            }
            return;
        }
    }

    public bool GetFishFromBag(out CollectedFish fish)
    {
        if (currentStack == 0)
        {
            fish = null;
            return false;
        }
        fish = fishInBagTemp[currentStack - 1];
        fishInBagTemp.RemoveAt(currentStack - 1);
        SetCapacity(currentStack - 1);
        return true;
    }

    public bool AddCatchFishToInventory(CollectedFish fish)
    {
        if (currentStack == maxCapacity.Value)
        {
            return false;
        }
        SetCapacity(currentStack + 1);
        fish.transform.SetParent(transform.GetChild(1));
        fish.transform.localEulerAngles = Vector3.zero;
        fish.transform.DOLocalJump(new Vector3(0, currentStack * bagSpacing, 0), 2, 1, 0.2f).OnComplete(() => { });
        fishInBagTemp.Add(fish);
        capacityText.transform.parent.gameObject.SetActive(true);
        return true;
    }

    void LateUpdate()
    {
        textParent.position = transform.position;
    }

    private void SetCapacity(int currentStack)
    {
        this.currentStack = currentStack;
        capacityText.SetText(currentStack + "/" + maxCapacity.Value);
        if (currentStack == 0)
        {
            capacityText.transform.parent.gameObject.SetActive(false);
        }
    }
}
