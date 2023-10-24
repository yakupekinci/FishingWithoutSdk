using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivaterUpgrade : MonoBehaviour
{

    public string laodKey;
    public GameObject objToActive;

    void Start()
    {
        if (PlayerPrefs.GetInt(laodKey, 0) == 1)
        {
            objToActive.SetActive(true);
        }
    }
}
