using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Door_Debug : MonoBehaviour
{
    public Vector3 doorSize;
    public Vector3 doorOffset;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, .5f, 1);
        Gizmos.DrawWireCube(transform.position + doorOffset, doorSize);
        Gizmos.DrawCube(transform.position + new Vector3(doorOffset.x, 0, doorOffset.z), new Vector3(doorSize.x, 0, doorSize.z));
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    }
}
