using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterFishArea : ActivateObjectOnTrigger
{
    [SerializeField] FishSpawner fishSpawner;
    bool secondAreaIsActive;
    public GameObject closeArea;
    public GameObject closeArea2;
    public Transform playerOutPos;
    [SerializeField] GameObject warnText;

    private void Awake()
    {
        secondAreaIsActive = PlayerPrefs.GetInt("secondAreaIsActive", 0) == 1;
        if (secondAreaIsActive)
        {
            Destroy(objToActivate);
        }
    }

    protected override void OnPlayerEnter(Collider other)
    {
        if (secondAreaIsActive)
        {
            //warnText.SetActive(true);
           EnterArea();
        }
        else
        {
            EnterArea();
        }
    }

    private void EnterArea()
    {
        closeArea.SetActive(false);
        objToActivate.SetActive(true);
        InGameUI.Instance.ShowFishGameUI();

        Vector3 targetPos = playerOutPos.position;
        targetPos.y = PlayerMovement.Instance.transform.position.y;

        PlayerMovement.Instance.transform.position = targetPos;
    }

    protected override void OnPlayerExit(Collider other)
    {
        base.OnPlayerExit(other);
        warnText.SetActive(false);
    }

    public void TryClearArea()
    {
        if (secondAreaIsActive)
            return;

        secondAreaIsActive = true;
        PlayerPrefs.SetInt("secondAreaIsActive", 1);
        fishSpawner.DestroyFishAll();
        Destroy(objToActivate);
    }

}
