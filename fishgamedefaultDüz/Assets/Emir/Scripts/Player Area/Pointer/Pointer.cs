using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pointer : MonoBehaviour
{

    public EnterFishArea enterFishArea;
    public CatchedFishArea catchedFishArea;
    public Int playerBagCurrent;
    public Int boatBagCurrent;
    public Transform sellArea;
    public Transform moneyArea;
    public Int boatBagMax;
    public Long playerGold;
    public ReduceGoldArea upgradePos;
    int phase;
    [SerializeField] private TMP_Text infoText;
    [SerializeField] private string[] info;
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private GameObject ppointer;
    [SerializeField] private GameObject GYM;
    [SerializeField] private GameObject OFFICE;
    [SerializeField] private GameObject PORT;
    [SerializeField] private GameObject FISHARES2;
    [SerializeField] private GameObject SHOP2;
    [SerializeField] private GameObject CANNED;
    [SerializeField] private GameObject CANNEDDROP;
    [SerializeField] private GameObject CANNEDBRING;
    [SerializeField] private GameObject cannedPlace;
    private bool shopBeforeExecuted = false;
    private bool shopExecuted = false;
    private bool gymExecuted = false;
    private bool officeExecuted = false;
    private bool portExecuted = false;
    private bool fishAreas2Executed = false;
    private bool shop2Executed = false;
    private bool cannedExecuted = false;
    private bool cannedExecuted2 = false;
    private bool cannedExecuted3 = false;
    private PlayerManager PlayerManager;
    int previousPhase;


    void Update()
    {


        if (phase == 5 && !gymExecuted)
        {
            GYMm();
            gymExecuted = true;

        }
        else if (phase == 6 && !officeExecuted && GYM.transform.root.transform.GetChild(2).transform.GetChild(1).gameObject.activeSelf)
        {
            OFFICEe();
            officeExecuted = true;

        }
        else if (phase == 7 && !portExecuted && OFFICE.transform.root.transform.GetChild(3).transform.GetChild(0).gameObject.activeSelf)
        {
            PORTt();
            portExecuted = true;

        }
        else if (phase == 8 && !fishAreas2Executed && PORT.transform.root.transform.GetChild(4).transform.GetChild(1).gameObject.activeSelf)
        {
            FISHARESs2();
            fishAreas2Executed = true;

        }
        else if (phase == 9 && !shop2Executed && FISHARES2.transform.root.transform.GetChild(5).transform.GetChild(1).gameObject.activeSelf)
        {
            SHOPp();
            shop2Executed = true;

        }
        else if (phase == 10 && !cannedExecuted && SHOP2.transform.root.transform.GetChild(6).transform.GetChild(1).gameObject.activeSelf)
        {
            CANNEDd();
            cannedExecuted = true;
        }
        else if (phase == 11 && !cannedExecuted2 && CANNED.transform.root.transform.GetChild(7).transform.GetChild(1).gameObject.activeSelf)
        {
            CANNEDdBring();
            cannedExecuted2 = true;
        }
        else if (phase == 12 && !cannedExecuted3 && CANNEDDROP.GetComponent<CannSpawner>().CannCount > 0)
        {
            CANNEDDrop();
            cannedExecuted3 = true;
        }
    }
    IEnumerator activeAREAS(GameObject area, int i)
    {
        ppointer.SetActive(true);
        transform.GetChild(0).gameObject.SetActive(true);
        Tutorial.SetActive(true);
        area.gameObject.SetActive(true);
        transform.position = area.transform.position + Vector3.up * 6;
        infoText.text = info[i];
        phase = i + 1;
        if (phase < 12 || phase > 13)
            PlayerPrefs.SetInt("pointerPhase", phase);
        yield return new WaitForSeconds(0.01f);
        //Tutorial.SetActive(false);
    }
    private void GYMm()
    {
        if (GYM.gameObject.activeSelf == true)
        {
            StartCoroutine(activeAREAS(GYM, 5));
        }
    }
    private void OFFICEe()
    {
        if (OFFICE.gameObject.activeSelf == true)
        {

            StartCoroutine(activeAREAS(OFFICE, 6));
        }
    }
    private void PORTt()
    {
        if (PORT.gameObject.activeSelf == true)
        {
            StartCoroutine(activeAREAS(PORT, 7));
        }
    }
    private void FISHARESs2()
    {
        if (FISHARES2.gameObject.activeSelf == true)
        {
            StartCoroutine(activeAREAS(FISHARES2, 8));
        }
    }
    private void SHOPp()
    {
        if (SHOP2.gameObject.activeSelf == true)
        {
            StartCoroutine(activeAREAS(SHOP2, 9));
        }
    }
    private void CANNEDd()
    {
        if (CANNED.gameObject.activeSelf == true)
        {
            StartCoroutine(activeAREAS(CANNED, 10));

        }
    }
    private void CANNEDdBring()
    {
        if (CANNEDBRING.gameObject.activeSelf == true)
        {
            StartCoroutine(activeAREAS(CANNEDBRING, 11));
            // StartCoroutine(InfoTextWaitTime());
        }
    }
    private void CANNEDDrop()
    {
        if (CANNEDDROP.gameObject.activeSelf == true)
        {
            StartCoroutine(activeAREAS(CANNEDDROP, 12));

        }
    }


    void Awake()
    {

        PlayerManager = FindObjectOfType<PlayerManager>();
        phase = PlayerPrefs.GetInt("pointerPhase", 0);

        if (phase == 5)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            //  Destroy(gameObject);
        }
        else if (phase == 4)
        {
            infoText.text = info[4];
            transform.position = upgradePos.transform.position + Vector3.up * 6;
        }
        else if (phase < 4)
        {
            transform.position = enterFishArea.transform.position + Vector3.up * 6;
            boatBagCurrent.OnValueChangedEvent.AddListener(CheckBotBag);
            infoText.text = info[0];
        }
        else if (phase == 6)
        {
            transform.position = GYM.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[5];
        }
        else if (phase == 7)
        {
            transform.position = OFFICE.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[6];
        }
        else if (phase == 8)
        {
            transform.position = PORT.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[7];
        }
        else if (phase == 9)
        {
            transform.position = FISHARES2.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[8];
        }
        else if (phase == 10)
        {
            transform.position = SHOP2.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[9];
        }
        else if (phase == 11)
        {
            transform.position = -CANNED.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[10];
        }
        else if (phase == 12)
        {
            phase = 11;
            transform.position = -CANNED.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[11];
        }

        else if (phase == 13)
        {
            phase = 11;
            transform.position = -CANNED.transform.position + Vector3.up * 6;
            Tutorial.SetActive(true);
            infoText.text = info[11];
        }


        else if (phase == 14)
        {

            Tutorial.SetActive(false);
            Destroy(Tutorial);
            transform.GetChild(0).gameObject.SetActive(false);
            ppointer.gameObject.SetActive(false);
            Destroy(ppointer.gameObject);
        }


    }

    private void CheckPlayerBag(int value)
    {
        if (value > 0 && phase == 1)
        {
            if (value + boatBagCurrent.Value >= boatBagMax.Value)
            {
                infoText.text = info[2];
                phase = 2;
                transform.position = sellArea.transform.position + Vector3.up * 6;
                playerBagCurrent.OnValueChangedEvent.RemoveListener(CheckPlayerBag);
                boatBagCurrent.OnValueChangedEvent.AddListener(CheckBotBag);
            }
        }
    }
    private void CheckBotBag(int value)
    {
        if (value == boatBagMax.Value && phase == 2)
        {
            infoText.text = info[3];
            boatBagCurrent.OnValueChangedEvent.RemoveListener(CheckBotBag);
            transform.position = moneyArea.transform.position + Vector3.up * 6;
            phase = 3;
        }
    }
    void OnEnable()
    {
        if (phase == 0 && playerBagCurrent.Value + catchedFishArea.CurrentAmount + boatBagCurrent.Value >= boatBagMax.Value && catchedFishArea.CurrentAmount > 0)
        {
            infoText.text = info[1];
            transform.position = catchedFishArea.transform.position + Vector3.up * 6;
            phase = 1;
            playerBagCurrent.OnValueChangedEvent.AddListener(CheckPlayerBag);
        }

        //Invoke("CloseOtherPointer", 5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (phase == 4 && !upgradePos.gameObject.activeSelf && !shopExecuted)
        {
            shopExecuted = true;
            phase = 5;
            infoText.text = info[5];
            PlayerPrefs.SetInt("pointerPhase", 5);
            transform.GetChild(0).gameObject.SetActive(false);
            ppointer.gameObject.SetActive(false);
        }
        else if (phase == 3 && playerGold.Value > 0 && !shopBeforeExecuted)
        {
            shopBeforeExecuted = true;
            infoText.text = info[4];
            phase = 4;
            PlayerPrefs.SetInt("pointerPhase", 4);
            transform.position = upgradePos.transform.position + Vector3.up * 6;
        }
        else if (phase == 6)
        {
            Tutorial.SetActive(false);
            ppointer.SetActive(false);
        }

        else if (phase == 7)
        {
            Tutorial.SetActive(false);
            ppointer.SetActive(false);
        }
        else if (phase == 8)
        {
            Tutorial.SetActive(false);
            ppointer.SetActive(false);
        }
        else if (phase == 9)
        {
            Tutorial.SetActive(false);
            ppointer.SetActive(false);
        }
        else if (phase == 10)
        {
            Tutorial.SetActive(false);
            ppointer.SetActive(false);
        }
        else if (phase == 11)
        {
            Tutorial.SetActive(false);
            ppointer.SetActive(false);
        }
        else if (phase == 12)
        {
            if (cannedPlace.GetComponent<Cannery>().fishCount != 0)
            {
                ppointer.SetActive(false);
                Tutorial.SetActive(false);
            }
        }

        else if (phase == 13)
        {

            if (CANNEDDROP.GetComponent<CannSpawner>().CannCount != -1)
            {
                ppointer.SetActive(false);
                Tutorial.SetActive(false);
                gameObject.SetActive(false);
                PlayerPrefs.SetInt("pointerPhase", 14);

            }
        }

    }
    IEnumerator InfoTextWaitTime()
    {

        yield return new WaitForSeconds(1f);
        Tutorial.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }
    IEnumerator InfoTextWaitTimefİSH2()
    {
        yield return new WaitForSeconds(10f);
        Tutorial.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
