using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bank : MonoBehaviour
{
    public TextMeshPro goldText;

    public void AddGold(int money)
    {
        PlayerManager.Instance.AddGold(money);
        AudioManager.Instance.PlayMoneyCollectAt(transform.position);
        goldText.SetText(money + "$");
        goldText.transform.parent.gameObject.SetActive(true);
        Invoke(nameof(CloseGoldText), 1f);
    }

    private void CloseGoldText()
    {
        goldText.transform.parent.gameObject.SetActive(false);
    }
}
