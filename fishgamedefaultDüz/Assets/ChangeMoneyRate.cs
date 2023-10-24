using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMoneyRate : MonoBehaviour
{
    [SerializeField] CollectGoldArea collectGoldArea;

    [SerializeField] int objPerMoney = 200;

    private void Start() {
        collectGoldArea.SetObjPerMoney(objPerMoney);
    }
}
