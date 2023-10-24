using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleMoneySpawner : Singleton<DoubleMoneySpawner>
{
    [SerializeField] GameObject moneyArea;
    float timer;
    [SerializeField] float waitTime = 120f;
    bool canSpawn = true;
    bool stopTimer;

    public void SetStopTimer(bool stopTimer)
    {
        this.stopTimer = stopTimer;
    }

    void Start()
    {
        moneyArea.SetActive(false);
        timer = 0;
    }

    public void Earned()
    {
        canSpawn = true;
        timer = 0;
    }

    private void Update()
    {
        if (stopTimer)
            return;

        timer += Time.deltaTime;
        if (canSpawn && timer >= waitTime)
        {
            canSpawn = false;
            moneyArea.SetActive(true);
        }
    }
    public void OpenCloseMoney()
    {
        moneyArea.SetActive(false);
    }
}
