using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundPlayer : MonoBehaviour
{
    public AudioSource footstep;
    public List<AudioClip> steps;

    public void PlayFootstep()
    {
        footstep.clip = steps[Random.Range(0, steps.Count)];
        footstep.Play();
    }
}
