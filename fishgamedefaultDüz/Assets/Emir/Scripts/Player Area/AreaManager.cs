using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : Singleton<AreaManager>
{
    public Transform cam;
    public Transform mainCam;

    private void OnEnable()
    {
        Instance = this;
        PlayerManager.Instance.SetStopTimer(true);
        DoubleMoneySpawner.Instance.SetStopTimer(true);

        MusicSource.Instance.target = cam;
    }

    private void OnDisable()
    {
        if (mainCam && MusicSource.Instance)
        {
            PlayerManager.Instance.SetStopTimer(false);
            DoubleMoneySpawner.Instance.SetStopTimer(false);
            MusicSource.Instance.target = mainCam;
        }
    }
}
