using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopArea : TriggerAreas
{
    
    protected override void OnPlayerEnter(Collider other)
    {
        base.OnPlayerEnter(other);
        InGameUI.Instance.ShowShopUI();
    }

}
