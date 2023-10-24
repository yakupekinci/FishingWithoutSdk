using UnityEngine;

[CreateAssetMenu(menuName = "FishSpawnList")]
public class FishSpawnSO : ScriptableObject
{
    public FishSpawnRow[] fishSpawnListColumn;
}

[System.Serializable]
public class FishSpawnRow
{
    public FishSpawn[] fishSpawnList;
}

[System.Serializable]
public class FishSpawn
{
    public Fish fish;
    public int startSpawnAmount;
}
