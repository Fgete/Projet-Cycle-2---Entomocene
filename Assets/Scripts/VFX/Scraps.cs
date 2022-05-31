using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scraps : MonoBehaviour
{
    public float fade;
    public float force;
    private Transform parent;
    private MeshRenderer mr;
    private Material mat;

    private void OnEnable()
    {
        mr = GetComponent<MeshRenderer>();
        mat = mr.material;
        parent = GameObject.Find("--- VFX ---").transform;
        if (parent)
            transform.parent = parent;
        GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) * force);
    }

    private void Update()
    {
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, mat.color.a - Time.deltaTime * fade);
        mr.material = mat;
        if (mr.material.color.a <= 0)
            Destroy(gameObject);
    }
}
