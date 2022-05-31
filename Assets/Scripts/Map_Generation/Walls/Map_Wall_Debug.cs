using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Wall_Debug : MonoBehaviour
{
    public Vector3 wallSize;
    public Vector3 wallOffset;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawCube(transform.position + wallOffset, wallSize);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
