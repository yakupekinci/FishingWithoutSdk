using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EndGameCalculator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldSpentTMP;
    [SerializeField] TextMeshProUGUI goldEarnedTMP;
    [SerializeField] TextMeshProUGUI timeSpentTMP;
    [SerializeField] TextMeshProUGUI rankTMP;
    [SerializeField] GameObject[] stars;

    int goldSpentTemp;
    int goldEarnedTemp;
    int timeSpentTemp;

    int goldSpent;
    int goldEarned;
    int timeSpent;

    void Awake()
    {
        goldSpent = PlayerPrefs.GetInt("goldSpentAll", 0);
        goldEarned = PlayerPrefs.GetInt("goldEarnedAll", 0);
        timeSpent = PlayerManager.Instance.GameTime;

        foreach (var star in stars)
        {
            star.SetActive(false);
        }
    }

    int rankTemp;
    int rank;

    void Start()
    {
        float percent = (float)timeSpent / 7200;
        float rankF = Mathf.Lerp(1, 30, percent);
        rank = ((int)rankF);
        rankTemp = 100;

        StartCoroutine(StarsAnim());
        StartChangeTextsAnim();

    }


    IEnumerator StarsAnim()
    {
        if (rank < 10)
        {
            yield return new WaitForSeconds(1f);
            stars[0].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[1].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[2].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[3].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[4].SetActive(true);
        }
        else if (rank < 20)
        {
            yield return new WaitForSeconds(1f);
            stars[0].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[1].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[2].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[3].SetActive(true);
    
        }
        else if (rank < 30)
        {
            yield return new WaitForSeconds(1f);
            stars[0].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[1].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[2].SetActive(true);
        }
        else if (rank < 40)
        {
            yield return new WaitForSeconds(1f);
            stars[0].SetActive(true);
            yield return new WaitForSeconds(1f);
            stars[1].SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            stars[0].SetActive(true);
        }
    }


    private void StartChangeTextsAnim()
    {
        DOTween.To(() => goldEarnedTemp, x => goldEarnedTemp = x, goldEarned, 3.0f)
           .OnUpdate(() =>
           {
               // Update the TextMeshProUGUI text during the animation
               goldEarnedTMP.SetText(goldEarnedTemp.ToString());
           });

        DOTween.To(() => goldSpentTemp, x => goldSpentTemp = x, goldSpent, 3.0f)
           .OnUpdate(() =>
           {
               // Update the TextMeshProUGUI text during the animation
               goldSpentTMP.SetText(goldSpentTemp.ToString());
           });

        DOTween.To(() => timeSpentTemp, x => timeSpentTemp = x, timeSpent, 3.0f)
           .OnUpdate(() =>
           {
               // Update the TextMeshProUGUI text during the animation
               if (timeSpentTemp >= 3600) // More than one hour
               {
                   int hours = Mathf.FloorToInt(timeSpentTemp / 3600);
                   int minutes = Mathf.FloorToInt((timeSpentTemp % 3600) / 60);
                   timeSpentTMP.SetText(hours + "h " + minutes + "m");
               }
               else // Less than one hour
               {
                   int minutes = Mathf.FloorToInt(timeSpentTemp / 60);
                   int seconds = Mathf.FloorToInt(timeSpentTemp % 60);
                   timeSpentTMP.SetText(minutes + "m " + seconds + "s");
               }
           });

        DOTween.To(() => rankTemp, x => rankTemp = x, rank, 3.0f)
           .OnUpdate(() =>
           {
               // Update the TextMeshProUGUI text during the animation
               rankTMP.SetText("RANK:  % " + rankTemp);
           });
    }


}
