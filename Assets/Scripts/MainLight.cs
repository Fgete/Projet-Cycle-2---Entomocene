using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLight : MonoBehaviour
{
    private void Start()
    {
        if (FindObjectOfType<Map_Generation_Manager>())
            GetComponent<Light>().color = FindObjectOfType<Map_Generation_Manager>().mainLightColor;
        Destroy(this);
    }
}
