using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public HookCollector hookCollector;
    public Transform[] fishSpawnAreas;
    public FishSpawnSO fishSpawnSO;

    List<FishPool> fishPools;

    void Start()
    {
        fishPools = new List<FishPool>();
        Vector3 spawnPos = Vector3.zero;

        for (int i = 0; i < fishSpawnAreas.Length; i++)
        {
            fishPools.Add(new FishPool());
            Vector3 spawnAreaPos = fishSpawnAreas[i].position;
            FishSpawnRow fishSpawnRow = fishSpawnSO.fishSpawnListColumn[i];
            for (int j = 0; j < fishSpawnRow.fishSpawnList.Length; j++)
            {
                FishSpawn fishSpawn = fishSpawnRow.fishSpawnList[j];
                for (int k = 0; k < fishSpawn.startSpawnAmount; k++)
                {
                    spawnPos = spawnAreaPos + new Vector3(Random.Range(-1f, 1f), Random.Range(-1.2f, 1.2f), 0);
                    fishPools[i].fishList.Add(Instantiate(fishSpawn.fish.gameObject, spawnPos, Quaternion.identity));
                }
            }
        }
    }

    public void DestroyFishAll()
    {
        if (fishPools == null)
            return;
        if (fishPools.Count > 0)
        {
            for (int i = fishPools.Count - 1; i >= 0; i--)
            {
                for (int k = fishPools[i].fishList.Count - 1; k >= 0; k--)
                {
                    Destroy(fishPools[i].fishList[k].gameObject);
                    fishPools[i].fishList.RemoveAt(k);
                }
                fishPools.RemoveAt(i);
            }
        }
        fishPools = null;
    }


    private void RespawnFish()
    {
        for (int i = 0; i < fishPools.Count; i++)
        {
            var spawnAreaPos = fishSpawnAreas[i].position;
            var deactiveFish = fishPools[i].fishList.FindAll(x => !x.activeSelf);
            foreach (var fish in deactiveFish)
            {
                bool right = Random.Range(0, 2) == 1;
                Vector3 targetPos;
                if (right)
                {
                    targetPos = spawnAreaPos + new Vector3(Random.Range(8f, 9f), Random.Range(-1.2f, 1.2f), 0);
                    fish.transform.eulerAngles = new Vector3(0, -90f, 0);
                }
                else
                {
                    targetPos = spawnAreaPos + new Vector3(Random.Range(-9f, -8f), Random.Range(-1.2f, 1.2f), 0);
                    fish.transform.eulerAngles = new Vector3(0, 90f, 0);
                }
                fish.transform.position = targetPos;
                fish.gameObject.SetActive(true);
            }
        }
    }

    void OnEnable()
    {
        hookCollector.OnFishReleased += RespawnFish;
    }

    void OnDestroy()
    {
        hookCollector.OnFishReleased -= RespawnFish;
    }
}

public class FishPool
{
    public List<GameObject> fishList;

    public FishPool()
    {
        fishList = new List<GameObject>();
    }
}


