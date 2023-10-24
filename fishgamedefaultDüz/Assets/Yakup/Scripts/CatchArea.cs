using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchArea : MonoBehaviour
{
    //Suyun Altına götürcek alan
    [SerializeField] private GameObject Player;


    private void Awake()
    {

    }



    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           
        }

    }

}

