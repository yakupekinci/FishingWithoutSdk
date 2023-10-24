using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using DG.Tweening;

public class WorkerSeller : MonoBehaviour
{
    enum WorkerSellerEnum
    {
        GoingToCannArea,
        Picking,
        GoingToSellArea,        
        ReleasingFish
    }


    [SerializeField] TextMeshPro capacityText;
    [SerializeField] Transform textParent;
    [SerializeField] Int maxCapacity;
    [SerializeField] float bagSpacing = 0.3f;
    NavMeshAgent agent;
    [SerializeField] Animator animator;
    List<CollectedFish> fishInBagTemp;
    [SerializeField] Float moveSpeed;
    int currentStack;

    [SerializeField] Transform sellAreaPos;
    [SerializeField] CannSpawner cannSpawner;
    [SerializeField] Transform cannAreaPosition;
    WorkerSellerEnum currentState;


    void Awake()
    {
        SetCapacity(0);
        agent = GetComponent<NavMeshAgent>();
        currentState = WorkerSellerEnum.GoingToCannArea;
        fishInBagTemp = new List<CollectedFish>();
    }

    private void Start()
    {
        PlayerPrefs.SetInt("workerSellerUnlocked", 1);

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
        if (currentState == WorkerSellerEnum.GoingToSellArea)
        {
            float distace = Vector3.Distance(transform.position, sellAreaPos.position);
            if (distace <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                currentState = WorkerSellerEnum.ReleasingFish;
                animator.SetBool("isRunning", false);
            }
            else
            {
                agent.SetDestination(sellAreaPos.position);
            }
            return;
        }

        if (currentState == WorkerSellerEnum.Picking)
        {
            if (currentStack == maxCapacity.Value)
            {
                agent.isStopped = false;
                currentState = WorkerSellerEnum.GoingToSellArea;
                animator.SetBool("isRunning", true);
            }
            else if (cannSpawner.CannCount == 0)
            {
                if (currentStack > 0)
                {
                    agent.isStopped = false;
                    currentState = WorkerSellerEnum.GoingToSellArea;
                    animator.SetBool("isRunning", true);
                }
            }
            else
            {
                // COLLECT LOGIC
            }
            return;
        }

        if (currentState == WorkerSellerEnum.GoingToCannArea)
        {
            float distace = Vector3.Distance(transform.position, cannAreaPosition.position);
            if (distace <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                currentState = WorkerSellerEnum.Picking;
                animator.SetBool("isRunning", false);
            }
            else
            {
                agent.SetDestination(cannAreaPosition.position);
            }
            return;
        }

        if (currentState == WorkerSellerEnum.ReleasingFish)
        {
            if (currentStack == 0)
            {
                agent.isStopped = false;
                currentState = WorkerSellerEnum.GoingToCannArea;
                animator.SetBool("isRunning", true);
            }
            else
            {
                // RELEASE LOGIC
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
