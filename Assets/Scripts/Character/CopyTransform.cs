using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CopyTransform : MonoBehaviour
{
    public bool execute = false;
    [Space]
    public Transform parent;
    [Space]
    public bool setPosition;
    public Vector3 deltaPosition;
    public bool setRotation;
    public Vector3 deltaRotation;
    public bool setScale;
    public Vector3 deltaScale;

    void Update()
    {
        if (!execute)
            return;

        if (setPosition)
            transform.position = parent.position + deltaPosition;
        if (setRotation)
            transform.eulerAngles = new Vector3(parent.rotation.x + deltaRotation.x, parent.rotation.y + deltaRotation.y, parent.rotation.z + deltaRotation.z);
        if (setScale)
            transform.localScale = new Vector3(parent.localScale.x * deltaScale.x, parent.localScale.y * deltaScale.y, parent.localScale.z * deltaScale.z);
    }
}
