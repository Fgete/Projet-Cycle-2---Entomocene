using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Light_Shaft : MonoBehaviour
{
    public Color colorLight = Color.white;
    public Gradient shaftLight;
    public List<Light> lights;
    public LineRenderer line;

    private void Start()
    {
        transform.eulerAngles = Vector3.zero;
        foreach (Light l in lights)
            l.color = colorLight;
        line.colorGradient = shaftLight;
    }
}
