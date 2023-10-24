using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fish : MonoBehaviour
{
    public FishSO fishSO;
    public int Capacity => fishSO.capacity;
    private FishMovement fishMovement;

    void Awake()
    {
        fishMovement = GetComponent<FishMovement>();
    }
    public void Catch()
    {
        AudioManager.Instance.PlayFishCollectAt(transform.position);
        fishMovement.StopMovement();
        transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        
    }

}
