using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish")]
public class FishSO : ScriptableObject
{
    public int capacity = 1;
    public int gold;
    public GameObject carryObject;
}
