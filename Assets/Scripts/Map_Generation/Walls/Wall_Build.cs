using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Build : MonoBehaviour
{
    public List<GameObject> prefabs;

    public void Build()
    {
        if (prefabs.Count > 0)
            Instantiate(prefabs[Random.Range(0, prefabs.Count)], transform.position, transform.rotation, transform);

        if (GetComponent<Map_Wall_Debug>())
            Destroy(GetComponent<Map_Wall_Debug>());

        if (GetComponent<Door_Build>())
            Destroy(GetComponent<Door_Build>());

        Destroy(GetComponent<Wall_Build>());
    }
}
