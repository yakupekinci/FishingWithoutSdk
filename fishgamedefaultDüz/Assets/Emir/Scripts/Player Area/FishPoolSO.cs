using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fish Pool SO")]
public class FishPoolSO : ScriptableObject
{
    public List<CatchedFishPoolObj> poolFishList;
}

[System.Serializable]
public class CatchedFishPoolObj
{
    public FishSO fishSO;
    public int poolSize;
}