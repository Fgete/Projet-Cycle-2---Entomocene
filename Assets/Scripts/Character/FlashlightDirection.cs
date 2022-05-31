using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightDirection : MonoBehaviour
{
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
            transform.LookAt(hit.point);
            // transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.parent.transform.eulerAngles.y, transform.eulerAngles.z);
        }
    }
}
