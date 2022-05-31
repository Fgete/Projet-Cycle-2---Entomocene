using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Death : MonoBehaviour
{
    [Header("PLAY")]
    public List<AudioSource> toPlay;

    [Header("STOP")]
    public List<AudioSource> toStop;

    [Header("ENABLE")]
    public List<GameObject> toEnable;

    [Header("DISABLE")]
    public List<GameObject> toDisable;

    private Character_Movement cm;
    private Character_Action ca;

    private void Awake()
    {
        cm = GetComponent<Character_Movement>();
        ca = GetComponent<Character_Action>();
    }

    public void Run()
    {
        // DISABLE SCRIPTS
        cm.enabled = false;
        ca.enabled = false;

        // PLAY
        foreach (AudioSource music in toPlay)
            music.Play();

        // STOP
        foreach (AudioSource music in toStop)
            music.Stop();

        // ENABLE
        foreach (GameObject go in toEnable)
            go.SetActive(true);

        // DISABLE
        foreach (GameObject go in toDisable)
            go.SetActive(false);
    }
}
