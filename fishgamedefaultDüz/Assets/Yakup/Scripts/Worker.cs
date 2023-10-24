using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum WorkerState
{
    wait,
    moveToCatchedFishArea,
    moveToSellArea,
    sell
}

public class Worker : MonoBehaviour
{
    private NavMeshAgent agent;
    public Float moveSpeed;
    private Animator animator;
    public WorkerBag workerBag;
    public CatchedFishArea catchedFishArea;
    public SellFishArea sellFishArea;
    public GameObject needFishTmp;
    WorkerState workerState;

    void Start()
    {
        PlayerPrefs.SetInt("workerUnlocked", 1);
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        if (workerBag.FishCount <= 0)
        {
            SetDestination(catchedFishArea.transform.position);
            workerState = WorkerState.moveToCatchedFishArea;
            animator.SetBool("isRunning", true);
        }

        agent.speed = moveSpeed.Value;

        moveSpeed.OnValueChangedEvent.AddListener(ChangeSpeed);
    }

    private void OnDestroy()
    {
        moveSpeed.OnValueChangedEvent.RemoveListener(ChangeSpeed);
    }

    private void ChangeSpeed(float value)
    {
        agent.speed = value;
    }

    [SerializeField] Transform catchedFishTransformm;
    [SerializeField] Transform sellFishTransformm;

    void Update()
    {
        if (workerState == WorkerState.moveToCatchedFishArea)
        {
            float distance = Vector3.Distance(transform.position, catchedFishTransformm.position);
            if (distance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                workerState = WorkerState.wait;
                animator.SetBool("isRunning", false);
            }
            else
            {
                agent.isStopped = false;
                SetDestination(catchedFishTransformm.position);
            }
        }
        else if (workerState == WorkerState.wait)
        {
            if (workerBag.IsFull)
            {
                if (needFishTmp)
                {
                    needFishTmp.SetActive(false);
                }
                workerState = WorkerState.moveToSellArea;
                SetDestination(sellFishTransformm.position);
                agent.isStopped = false;
                animator.SetBool("isRunning", true);
            }
            else if (catchedFishArea.fishCount <= 0)
            {
                if (workerBag.FishCount > 0)
                {
                    if (needFishTmp)
                    {
                        needFishTmp.SetActive(false);
                    }
                    workerState = WorkerState.moveToSellArea;
                    agent.isStopped = false;
                    SetDestination(sellFishTransformm.position);
                    animator.SetBool("isRunning", true);
                }
                else if (needFishTmp)
                {
                    needFishTmp.SetActive(true);
                }
            }
            else
            {
                if (needFishTmp)
                {
                    needFishTmp.SetActive(false);
                }
            }
        }
        else if (workerState == WorkerState.moveToSellArea)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                workerState = WorkerState.sell;
                animator.SetBool("isRunning", false);
            }
            else
            {
                agent.isStopped = false;
                SetDestination(sellFishTransformm.position);
            }
        }
        else
        {
            if (workerBag.FishCount == 0)
            {
                workerState = WorkerState.moveToCatchedFishArea;
                agent.isStopped = false;
                SetDestination(catchedFishTransformm.position);
                animator.SetBool("isRunning", true);
            }
        }

        if (workerBag.FishCount == 0)
        {
            float distanceToPickArea = Vector3.Distance(transform.position, catchedFishTransformm.position);
            if (distanceToPickArea <= agent.stoppingDistance)
            {
                if (workerState != WorkerState.wait)
                {
                    agent.isStopped = true;
                    workerState = WorkerState.wait;
                    animator.SetBool("isRunning", false);
                }
            }
            else if (workerState != WorkerState.moveToCatchedFishArea)
            {
                workerState = WorkerState.moveToCatchedFishArea;
                agent.isStopped = false;
                SetDestination(catchedFishTransformm.position);
                animator.SetBool("isRunning", true);
            }
        }
        else if (workerBag.IsFull && workerState != WorkerState.moveToSellArea)
        {
            if (needFishTmp)
            {
                needFishTmp.SetActive(false);
            }
            workerState = WorkerState.moveToSellArea;
            SetDestination(sellFishTransformm.position);
            agent.isStopped = false;
            animator.SetBool("isRunning", true);
        }
    }


    void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
