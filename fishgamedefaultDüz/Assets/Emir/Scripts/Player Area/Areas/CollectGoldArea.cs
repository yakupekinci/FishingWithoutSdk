using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CollectGoldArea : MonoBehaviour
{
    public RectTransform moneyToCloneInScene;
    List<RectTransform> moneyIconList;
    public RectTransform moneyIconMoveTarget;
    [SerializeField] Banker banker;
    public GameObject moneyPrefab;
    public int poolCount = 70;
    public int objPerMoney = 100;
    List<GameObject> moneyPool;
    List<Transform> moneyList;
    public Transform moneyMoveStart;
    [SerializeField] Transform startPos;
    Sequence sequence;
    int currentMoney;
    Sequence iconSeq;
    public Camera gameCam;
    public TMP_Text moneyTableText;
    [SerializeField] private TMP_Text earnedMoneyTMP;
    [SerializeField] private GameObject earnedGameOBJ;
    

    public int MONEYCOUNT => lastIndex + 1;

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

        moneyIconList = new List<RectTransform>();
        for (int i = 0; i < 20; i++)
        {
            moneyIconList.Add(Instantiate(moneyToCloneInScene.gameObject, moneyToCloneInScene.parent).GetComponent<RectTransform>());
        }
        Destroy(moneyToCloneInScene.gameObject);

        objPerMoney = PlayerPrefs.GetInt("capacityAreaParent", 0) == 1 ? 200 : 50;
    }

    public void SetObjPerMoney(int objPerMoney)
    {
        if (objPerMoney > this.objPerMoney)
            this.objPerMoney = objPerMoney;
    }

    private void CollectMoneyPlayer()
    {

        AudioManager.Instance.PlayMoneyCollectAt(transform.position);
        if (sequence != null)
        {
            sequence.Kill(true);
        }

        iconSeq = DOTween.Sequence();
        var tempList = new List<Transform>(moneyList);
        moneyList.Clear();
        // earnedMoneyTMP.text = MONEYCOUNT.ToString();
        lastIndex = -1;
        PlayerManager.Instance.AddGold(currentMoney);
        currentMoney = 0;
        foreach (var mon in tempList)
        {
            var moneyIcon = moneyIconList.Find(x => !x.gameObject.activeSelf);
            if (moneyIcon)
            {

                moneyIcon.gameObject.SetActive(true);

                // Calculate the position of mon on the 2D viewport
                Vector2 startPos = gameCam.WorldToScreenPoint(mon.position);

                // Set the initial local position of the moneyIcon
                moneyIcon.transform.position = startPos;

                // Move the moneyIcon slightly upward before moving to the target position
                Vector2 targetPos = moneyIconMoveTarget.anchoredPosition;
                float upPos = moneyIcon.localPosition.y + 100f;
                moneyIcon.DOLocalMoveY(upPos, Random.Range(0.2f, 0.4f)).OnComplete(() =>
                {
                    // Move the moneyIcon to the target position
                    MoveMoneyIconToPosition(moneyIcon, targetPos);
                });

            }
            mon.gameObject.SetActive(false);
            moneyTableText.text = MONEYCOUNT.ToString();
        }
    }

    bool isCollecting;
    private void OnTriggerStay(Collider other)
    {
        if (isCollecting)
            return;

        if (other.CompareTag("Player"))
        {
            isCollecting = true;
            if (currentMoney > 0)
            {
                CollectMoneyPlayer();
            }
            isCollecting = false;
            return;
        }
        else if (other.CompareTag("Banker"))
        {
            if (currentMoney > 0 && moneyList.Count > 0 && bankerTimer <= Time.time)
            {
                bankerTimer = Time.time + 0.15f;
                if (banker.CanCollect())
                {
                    sequence.Complete();
                    isCollecting = true;
                    lastIndex--;
                    int index = moneyList.Count - 1;
                    Transform mon = moneyList[index];
                    moneyList.RemoveAt(index);

                    int moneyAmount = Mathf.Min(currentMoney, objPerMoney);
                    currentMoney -= moneyAmount;
                    banker.CollectMoney(mon, moneyAmount);
                    isCollecting = false;
                    moneyTableText.text = MONEYCOUNT.ToString();
                }
            }
        }

    }

    float bankerTimer;

    private void MoveMoneyIconToPosition(RectTransform moneyIcon, Vector2 targetPosition)
    {
        moneyIcon.DOLocalMove(targetPosition, Random.Range(0.4f, 0.8f)).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            moneyIcon.gameObject.SetActive(false); // Deactivate the moneyIcon after the animation
        });
    }

    public void AddTotal(long total)
    {
        MoneyMoney(total);
    }

    public void AddMoney(int money)
    {
        currentMoney += money;
        int neededObjCount = currentMoney / objPerMoney;
        int objCount = moneyList.Count;
        if (neededObjCount > objCount)
        {
            for (int i = objCount; i < neededObjCount; i++)
            {
                GameObject mon = moneyPool.Find(x => !x.activeSelf);
                if (mon)
                {
                    PlaceMoney(mon.transform);
                    mon.SetActive(true);
                    moneyList.Add(mon.transform);
                }
            }
        }
        else if (currentMoney > 0 && objCount == 0)
        {
            GameObject mon = moneyPool.Find(x => !x.activeSelf);
            if (mon)
            {
                PlaceMoney(mon.transform);
                mon.SetActive(true);
                moneyList.Add(mon.transform);
            }
        }
        moneyTableText.text = MONEYCOUNT.ToString();
    }

    private void PlaceMoney(Transform mon)
    {
        mon.SetParent(transform);
        mon.eulerAngles = Vector3.zero;
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

        targetPos = targetPos + new Vector3(columnIndex * 1.7f, layer * 0.6f, rowIndex * 1.3f);
        return targetPos;
    }

    private void MoneyMoney(long total)
    {
        earnedGameOBJ.SetActive(true);
        earnedMoneyTMP.text = "+" + total.ToString();
        StartCoroutine(EarnedGold());
    }
    IEnumerator EarnedGold()
    {
        yield return new WaitForSeconds(1.5f);
        earnedGameOBJ.SetActive(false);
    }
}
