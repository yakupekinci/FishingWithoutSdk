using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterFishAreaSecond : ActivateObjectOnTrigger
{
    [SerializeField] EnterFishArea enterFishArea;
    [SerializeField] GameObject closeArea;
    [SerializeField] GameObject closeArea2;
    [SerializeField] GameObject UI;
    [SerializeField] Transform playerOutPos;

    private void Start()
    {
        UI.SetActive(true);
    }

    protected override void OnPlayerEnter(Collider other)
    {
        EnterArea();
    }

    private void EnterArea()
    {
        closeArea.SetActive(false);
        objToActivate.SetActive(true);
        InGameUI.Instance.ShowFishGameUI();


        Vector3 targetPos = playerOutPos.position;
        targetPos.y = PlayerMovement.Instance.transform.position.y;

        PlayerMovement.Instance.transform.position = targetPos;

       // enterFishArea.TryClearArea();
    }
}
