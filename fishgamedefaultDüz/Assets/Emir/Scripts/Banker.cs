using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using TMPro;

public class Banker : MonoBehaviour
{

    enum BankerStates
    {
        WaitingMoney,
        MovingToBank,
        MovingToMoneyTable
    }

    private NavMeshAgent agent;
    [SerializeField] CollectGoldArea collectGoldArea;
    [SerializeField] Animator animator;
    [SerializeField] Transform targetPos;
    [SerializeField] Transform bankPos;
    [SerializeField] Transform backPos;
    [SerializeField] Bank bank;
    [SerializeField] Int stackCapacity;
    int currentStack = 0;
    bool isMoving;
    int money;
    [SerializeField] Float moveSpeed;
    List<Transform> moneyList;
    [SerializeField] TextMeshPro capacityText;
    bool canCollect = true;
    BankerStates currentState;

    private void Start()
    {
        PlayerPrefs.SetInt("bankerUnlocked", 1);
        agent = GetComponent<NavMeshAgent>();
        SetDestination(targetPos.position);
        isMoving = true;
        animator.SetBool("isRunning", true);
        moneyList = new List<Transform>();
        moveSpeed.OnValueChangedEvent.AddListener(ChangeSpeed);
        agent.speed = moveSpeed.Value;
        capacityText.SetText(currentStack + "/" + stackCapacity.Value);
        currentState = BankerStates.MovingToMoneyTable;
    }

    private void OnDisable()
    {
        moveSpeed.OnValueChangedEvent.RemoveListener(ChangeSpeed);
    }

    private void ChangeSpeed(float speed)
    {
        agent.speed = speed;
    }

    bool isCollecting;

    public bool Collect(List<Transform> moneyList, int money)
    {
        if (isCollecting)
            return false;

        if (isMoving)
            return false;

        isCollecting = true;
        this.money = money;
        foreach (var mon in moneyList)
        {
            this.moneyList.Add(mon);
        }
        StartCoroutine(MoveMoneyToBag());
        animator.SetBool("isCollecting", true);
        return true;
    }

    public bool CanCollect()
    {
        return moneyList.Count < stackCapacity.Value && !isMoving && canCollect;
    }

    public void CollectMoney(Transform money, int moneyAmount)
    {
        isCollecting = true;
        animator.SetBool("isCollecting", true);
        this.money += moneyAmount;
        moneyList.Add(money);
        money.SetParent(backPos);
        money.localEulerAngles = Vector3.zero;
        Vector3 moveTarget = new Vector3(0, (moneyList.Count - 1) * 0.3f, 0);
        money.DOLocalMove(moveTarget, 0.05f).OnComplete(AddMoneyToBag);
    }

    private void AddMoneyToBag()
    {
        currentStack++;
        capacityText.SetText(currentStack + "/" + stackCapacity.Value);
        if (currentStack == stackCapacity.Value)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isCollecting", false);
            agent.isStopped = false;
            isMoving = true;
            SetDestination(bankPos.position);
            canCollect = false;
        }
        else if (collectGoldArea.lastIndex == -1)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isCollecting", false);
            agent.isStopped = false;
            isMoving = true;
            SetDestination(bankPos.position);
            canCollect = false;
        }
    }

    IEnumerator MoveMoneyToBag()
    {
        WaitForSeconds timer = new WaitForSeconds(0.07f);
        Vector3 pos = Vector3.zero;
        for (int i = moneyList.Count - 1; i >= 0; i--)
        {
            var mon = moneyList[i];
            mon.SetParent(backPos);
            mon.localEulerAngles = Vector3.zero;
            pos.y += 0.3f;
            mon.transform.DOLocalMove(pos, 0.05f);
            yield return timer;
        }
        animator.SetBool("isRunning", true);
        animator.SetBool("isCollecting", false);
        agent.isStopped = false;
        isMoving = true;
        SetDestination(bankPos.position);
    }


    void Update()
    {

        if (currentState == BankerStates.MovingToMoneyTable)
        {
            float distance = Vector3.Distance(transform.position, targetPos.position);
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                isMoving = false;
                animator.SetBool("isRunning", false);
                currentState = BankerStates.WaitingMoney;
            }
            else
            {
                SetDestination(targetPos.position);
            }
            return;
        }

        if (currentState == BankerStates.WaitingMoney)
        {
            if (!canCollect)
            {
                agent.isStopped = false;
                animator.SetBool("isRunning", true);
                animator.SetBool("isCollecting", false);
                currentState = BankerStates.MovingToBank;
            }
            else if (collectGoldArea.lastIndex == -1 && currentStack > 0)
            {
                agent.isStopped = false;
                animator.SetBool("isRunning", true);
                animator.SetBool("isCollecting", false);
                currentState = BankerStates.MovingToBank;
            }
            return;
        }

        if (currentState == BankerStates.MovingToBank)
        {
            float distance = Vector3.Distance(transform.position, bankPos.position);
            if (distance <= agent.stoppingDistance && money == 0)
            {
                SetDestination(targetPos.position);
                currentState = BankerStates.MovingToMoneyTable;
            }
            else
            {
                agent.isStopped = false;
                SetDestination(bankPos.position);
            }
        }

    }

    void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MoneyTable"))
        {
            if (collectGoldArea.lastIndex == -1 && currentStack > 0 && isMoving == false)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isCollecting", false);
                agent.isStopped = false;
                isMoving = true;
                SetDestination(bankPos.position);
                canCollect = false;
            }
            else if (!isMoving && !isCollecting)
            {
                SetDestination(targetPos.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bank>(out var bank))
        {
            if (money <= 0)
                return;

            isCollecting = false;
            bank.AddGold(money);
            money = 0;
            for (int i = 0; i < moneyList.Count; i++)
            {
                moneyList[i].gameObject.SetActive(false);
            }
            moneyList.Clear();
            currentStack = 0;
            canCollect = true;
            capacityText.SetText(currentStack + "/" + stackCapacity.Value);
            SetDestination(targetPos.position);
            currentState = BankerStates.MovingToMoneyTable;
        }
    }
}
