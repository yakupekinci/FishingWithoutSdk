using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public enum FisherStates
{
    Wait,
    MoveToFishCatchLake,
    MoveToSellArea,
    Sell
}

public class FisherMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    public Transform moveTarget;
    public CatchedFishArea fishCatchLakeArea;
    public GameObject needFishTmp;
    public GameObject walkROD;
    public GameObject stopROD;
    FisherStates fisherStates = FisherStates.MoveToFishCatchLake;
    public Float catchWaitTime;
    public Int maxFishToCatch;
    public int mainFishToCatch = 3;
    private float catchTimer = 0f;
    [SerializeField] Vector3 targetAngle = new Vector3(0, -180, 0);
    [SerializeField] string saveName = "fisherUnlocked";

    private void Awake() {
        agent = GetComponent<NavMeshAgent>();
        SetDestination(moveTarget.position);
    }

    void Start()
    {
        PlayerPrefs.SetInt(saveName, 1);
        animator = transform.GetChild(0).GetComponent<Animator>();

        if (agent != null)
        {
            SetDestination(moveTarget.position);
            animator.SetBool("isRunning", true);
            walkROD.SetActive(true);
            stopROD.SetActive(false);
            fisherStates = FisherStates.MoveToFishCatchLake;
        }
    }

    void Update()
    {
        if (fisherStates == FisherStates.MoveToFishCatchLake)
        {
            float distance = Vector3.Distance(transform.position, moveTarget.position);
            if (distance <= agent.stoppingDistance)
            {
                agent.isStopped = true;
                transform.DORotate(targetAngle, 1f);
                fisherStates = FisherStates.Wait;
                transform.eulerAngles = moveTarget.eulerAngles;
                animator.SetBool("isRunning", false);
                walkROD.SetActive(false);
                stopROD.SetActive(true);
            }
            else
            {
                agent.isStopped = false;
                SetDestination(moveTarget.position);
                animator.SetBool("isRunning", true);
                walkROD.SetActive(true);
                stopROD.SetActive(false);
            }
        }
        else if (fisherStates == FisherStates.Wait)
        {

            catchTimer += Time.deltaTime;

            if (catchTimer >= catchWaitTime.Value)
            {
                StartCoroutine(FisherCatchFish());
                catchTimer = 0f;
            }
        }
    }

    IEnumerator FisherCatchFish()

    {
        WaitForSeconds timer = new WaitForSeconds(0.2f);
        for (int i = 0; i < Random.Range(mainFishToCatch, maxFishToCatch.Value); i++)
        {
            int randomFishIndex = Random.Range(0, fishCatchLakeArea.fishPoolLists.Count);
            fishCatchLakeArea.AddFishFisher(fishCatchLakeArea.fishPoolLists[randomFishIndex].fishSO, transform);
            yield return timer;
        }
    }

    void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
