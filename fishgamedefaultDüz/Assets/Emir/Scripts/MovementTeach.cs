using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MovementTeach : MonoBehaviour
{
    const string saveName = "howToMove";
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] private GameObject Ways;
    [SerializeField] private GameObject Pponiter;
    [SerializeField] private GameObject Poniter;
    [SerializeField] private GameObject Tutorial;
    [SerializeField] private TMP_Text Timer;
    bool isDone;
    public float visualTimer = 10f;
    private void Awake()
    {
        isDone = PlayerPrefs.GetInt(saveName, 0) == 1;
        if (isDone)
        {
            Destroy(gameObject);
            Tutorial.SetActive(true);
            Pponiter.SetActive(true);
            Poniter.SetActive(true);
            Ways.SetActive(true);
            Timer.gameObject.SetActive(false);
        }
        else
        {
            Tutorial.SetActive(false);
            Pponiter.SetActive(false);
            Poniter.SetActive(false);
            Ways.SetActive(false);
            Timer.gameObject.SetActive(true);

        }

    }

    private void Update()
    {
        visualTimer -= Time.deltaTime;
        int timerValue = Mathf.FloorToInt(visualTimer);
        Timer.text = timerValue.ToString();
        if (visualTimer <= 0f && !isDone)
        {
            Timer.gameObject.SetActive(false);
            isDone = true;
            PlayerPrefs.SetInt(saveName, 1);
            canvasGroup.DOFade(0f, 1f).OnComplete(DestroyCanvas);
        }
    }

    private void DestroyCanvas()
    {

        Ways.SetActive(true);
        Pponiter.SetActive(true);
        Poniter.SetActive(true);
        Tutorial.SetActive(true);
        Destroy(gameObject);
        Timer.gameObject.SetActive(false);

    }

}
