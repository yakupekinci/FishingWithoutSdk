using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private TMP_Text earnedMoneyTMPUI;
    [SerializeField] private GameObject earnedGameOBJUI;
    public TextMeshProUGUI goldTMP;
    public Long gold; 
  
    float gameTimer;

    protected override void Awake()
    {
        base.Awake();
        if (PlayerPrefs.GetInt("pointerPhase", 0) >= 3)
        {
            gold.Value = PlayerPrefs.GetInt("playerGold", 0);
        }
        else
        {
            gold.Value = 0;
        }

        gameTimer = PlayerPrefs.GetInt("timeSpent", 0);
    }

    private void Start()
    {
        goldTMP.SetText(gold.Value.ToString());
    }

    void Update()
    {
        gameTimer += Time.deltaTime;

        earnedGoldTimer -= Time.deltaTime;
        if (earnedGoldTimer <= 0f)
        {
            earnedGameOBJUI.SetActive(false);
        }
    }

    public int GameTime => ((int)gameTimer);

    void OnDisable()
    {
        PlayerPrefs.SetInt("timeSpent", ((int)gameTimer));
        PlayerPrefs.Save();
    }

    public void ReduceGold(long amount)
    {
        gold.Value -= amount;
        goldTMP.SetText(gold.Value.ToString());
        PlayerPrefs.SetInt("playerGold", (int)gold.Value);

        int goldSpent = PlayerPrefs.GetInt("goldSpentAll", 0);
        PlayerPrefs.SetInt("goldSpentAll", goldSpent + ((int)amount));
    }

    float earnedGoldTimer;
    const float SHOW_EARNED_GOLD_TIME = 1.5f;

    public void AddGold(long amount)
    {
        earnedMoneyTMPUI.SetText("+" + amount.ToString());
        earnedGoldTimer = SHOW_EARNED_GOLD_TIME;
        earnedGameOBJUI.SetActive(true);

        gold.Value += ((long)(amount * multiplier));
        goldTMP.SetText(gold.Value.ToString());
        int total = (int)gold.Value;
        PlayerPrefs.SetInt("playerGold", total);

        int goldEarned = PlayerPrefs.GetInt("goldEarnedAll", 0);
        int last = goldEarned + ((int)(amount * multiplier));
        PlayerPrefs.SetInt("goldEarnedAll", last);

        PlayerPrefs.Save();
    }

    private float multiplier = 1f;
    float timer;

    [SerializeField] CanvasGroup happyTimeUI;

    public void EarnMoreMoney(float time)
    {
        multiplier = 2f;
        timer = time;
        happyTimeUI.alpha = 1f;
        happyTimeUI.gameObject.SetActive(true);
        timerText.SetText(((int)time).ToString());

        StartCoroutine(HappyTimeCoroutine());
    }

    private void FadeOutHappy()
    {
        happyTimeUI.DOFade(0f, 2f);
    }

    [SerializeField] TextMeshProUGUI timerText;
    bool stopTimer;
    public void SetStopTimer(bool stopTimer)
    {
        this.stopTimer = stopTimer;
    }

    IEnumerator HappyTimeCoroutine()
    {
        var wfs = new WaitForSeconds(1f);
        while (timer >= 0)
        {
            yield return wfs;
            if (!stopTimer)
            {
                timer -= 1;
                if (timer == 2)
                {
                    FadeOutHappy();
                }
                timerText.SetText(((int)timer).ToString());
            }

        }
        multiplier = 1f;
        happyTimeUI.gameObject.SetActive(false);
    }


}
