using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            UI_FadeIn uiEnd = Resources.FindObjectsOfTypeAll<UI_FadeIn>()[0];
            uiEnd.gameObject.SetActive(true);
            uiEnd.Run();
        }
    }
}
