using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameArea : TriggerAreas
{
    [SerializeField] GameObject endGameUI;
    [SerializeField] GameObject parent;
    bool isActive;

    protected override void OnPlayerEnter(Collider other)
    {
        base.OnPlayerEnter(other);
        if (other.CompareTag("Player"))
        {
            if (isActive)
                return;

            isActive = true;

            endGameUI.SetActive(true);
            PlayerPrefs.SetInt("hasEndGameShown", 1);

            PlayerMovement.Instance.DisableMovement();
            InGameUI.Instance.ActiveUI = endGameUI;
            AdManager.Instance.ShowHappyTime();
            parent.SetActive(false);
        }
    }


    public void OnCancelPressed()
    {
        endGameUI.SetActive(false);

        AudioManager.Instance.PlayUISoundAtGameCam();
        PlayerMovement.Instance.EnableMovement();

    }


}
