using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectConserveArea : MonoBehaviour
{

    [SerializeField] private FabricaBag fabricaBag;

    [SerializeField] GameObject stackIsFullTxt;
    public GameObject moneyPrefab;
    public int poolCount = 70;
    public int objPerMoney = 100;
    public List<GameObject> moneyPool;
    public List<Transform> moneyList;
    public Transform moneyMoveStart;
    [SerializeField] Transform startPos;
    Sequence sequence;
    int currentMoney;
    Sequence iconSeq;


    void Awake()
    {
        moneyList = new List<Transform>();
        moneyPool = new List<GameObject>();
        totalPositions = rowCount * columnCount;
        GameObject money;
        for (int i = 0; i < poolCount; i++)
        {
            money = Instantiate(moneyPrefab);
            money.SetActive(false);
            money.transform.SetParent(transform);
            moneyPool.Add(money);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentMoney > 0)
            {
                AudioManager.Instance.PlayMoneyCollectAt(transform.position);
                if (sequence != null)
                {
                    sequence.Kill(true);
                }
                foreach (var mon in moneyList)
                {

                    mon.gameObject.SetActive(false);
                }
                moneyList.Clear();
                lastIndex = -1;
                PlayerManager.Instance.AddGold(currentMoney);
                currentMoney = 0;
            }
        }
    }


    public void AddMoney(int gold)
    {

        GameObject mon = moneyPool.Find(x => !x.activeSelf);
        if (mon != null && lastIndex < fabricaBag.storageCapacity.Value - 1)
        {
            lastIndex++;
            CheckIsFull();
            mon.transform.position = transform.position;
            mon.transform.SetParent(transform);
            mon.gameObject.SetActive(true);
            PlaceMoney(mon.transform);
            mon.SetActive(true);
            moneyList.Add(mon.transform);
            mon.transform.DOLocalJump(CalculateTargetPos(), 2f, 1, 0.2f).OnComplete(() =>
            {

            });

        }

    }
    private void CheckIsFull()
    {
        if (lastIndex < fabricaBag.storageCapacity.Value - 1)
        {
            if (stackIsFullTxt.activeSelf)
            {
                stackIsFullTxt.SetActive(false);
            }
        }
        else if (!stackIsFullTxt.activeSelf)
        {
            stackIsFullTxt.SetActive(true);

        }
    }
    private void PlaceMoney(Transform mon)
    {
        if (sequence != null)
        {
            mon.localPosition = moneyMoveStart.localPosition;
            sequence.Join(mon.transform.DOLocalMove(CalculateTargetPos(), Random.Range(0.1f, 0.2f)));
        }
        else
        {
            sequence = DOTween.Sequence();
            mon.localPosition = moneyMoveStart.localPosition;
            sequence.Join(mon.transform.DOLocalMove(CalculateTargetPos(), Random.Range(0.1f, 0.2f)));
            sequence.OnComplete(() => sequence = null);
        }
    }

    public int rowCount = 3;
    public int columnCount = 3;
    private int totalPositions;
    public int lastIndex = -1;

    private Vector3 CalculateTargetPos()
    {
        lastIndex++;
        Vector3 targetPos = startPos.localPosition;
        int layer = lastIndex / totalPositions; // Katman hesaplaması
        int tempLastIndex = lastIndex % totalPositions; // Pozisyon hesaplaması

        int rowIndex = tempLastIndex / columnCount;
        int columnIndex = tempLastIndex % columnCount;

        targetPos = targetPos + new Vector3(columnIndex * 1.5f, layer * 0.5f, rowIndex * 1.5f);
        return targetPos;
    }
}
