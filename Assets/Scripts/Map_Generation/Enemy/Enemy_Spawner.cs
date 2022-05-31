using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [Range(0, 100)]
    public int objectSpawnProbability = 50;
    public List<GameObject> prefabs;

    private void Start()
    {
        if (Random.Range(0, 100) < objectSpawnProbability)
            if (transform.childCount == 0)
                if (prefabs.Count != 0)
                {
                    GameObject prefab = prefabs[Random.Range(0, prefabs.Count)];
                    Instantiate(prefab, transform.position, Quaternion.Euler(0, Random.Range(0, 360), 0), transform);
                }
        Destroy(this);
    }
}
