using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletOutSpotDirection : MonoBehaviour
{
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(worldPosition);
        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            transform.LookAt(new Vector3(hit.point.x, hit.point.y, hit.point.z));
            // transform.LookAt(hit.point);
            // transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        */
    }
}
