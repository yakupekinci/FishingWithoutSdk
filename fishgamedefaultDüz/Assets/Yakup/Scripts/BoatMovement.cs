using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class BoatMovement : MonoBehaviour
{
    [SerializeField] private BoatBag boatBag;
    public bool isStop = true;
    public Transform forwardTarget;
    public Transform waitTarget;
    public Transform startTarget;
    public Float waitTime;
    public GameObject bagNumUI;

    private void StartOver()
    {
        isStop = true;
        boatBag.ResetCount();
        bagNumUI.SetActive(true);
        transform.position = startTarget.position;
        transform.DOMove(waitTarget.position, waitTime.Value / 2).OnComplete(() =>
            boatBag.AfterLand());
    }

    public bool Leave()
    {
        if (!isStop)
            return false;

        isStop = false;
        bagNumUI.SetActive(false);
        transform.DOMove(forwardTarget.position, waitTime.Value / 2);
        transform.DOScale(transform.localScale, waitTime.Value).OnComplete(StartOver);
        return true;
        
        
    }



}
