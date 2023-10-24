using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockControl : MonoBehaviour
{
    [SerializeField] string saveName;

    private void Awake() {
        if (PlayerPrefs.GetInt(saveName, 0) == 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OpenAndUnlock()
    {
        PlayerPrefs.SetInt(saveName, 1);
        gameObject.SetActive(true);
    }


}
