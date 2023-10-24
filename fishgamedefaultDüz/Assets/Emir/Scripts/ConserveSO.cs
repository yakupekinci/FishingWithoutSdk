using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conserve")]
public class ConserveSO : ScriptableObject
{
    public int capacity = 1;
    public int gold;
    public GameObject carryObject;
}
