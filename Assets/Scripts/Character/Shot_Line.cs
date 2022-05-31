using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Line : MonoBehaviour
{
    public float fade;
    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    void Update()
    {
        lr.material.color = new Color(lr.material.color.r, lr.material.color.g, lr.material.color.b, lr.material.color.a - Time.deltaTime * fade);
        if (lr.material.color.a <= 0)
            Destroy(gameObject);
    }
}
