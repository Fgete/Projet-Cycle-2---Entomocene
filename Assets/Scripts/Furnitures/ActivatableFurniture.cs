using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatableFurniture : MonoBehaviour
{
    [Header("DROPS")]
    [Range(0, 100)]
    public int dropProbability;
    public List<GameObject> collectables;
    public Transform collectableSpawnPoint;

    [Header("ANIMATIONS")]
    public Animator animator;

    [Header("SOUNDS")]
    public AudioSource breakSound;
    public List<AudioClip> breakClips;

    [Header("VFX")]
    public ParticleSystem particles;

    public void Activate()
    {
        // PLAY SOUND
        if (breakClips.Count > 0)
        {
            breakSound.clip = breakClips[Random.Range(0, breakClips.Count)];
            breakSound.Play();
        }
        
        // ANIMATION
        if (animator)
            animator.SetTrigger("Open");

        // PARTICLE IN VFX PARENT
        if (particles)
            Instantiate(particles, collectableSpawnPoint.position + new Vector3(0, 1, 0), Quaternion.Euler(0, Random.Range(0, 360), 0), GameObject.Find("--- VFX ---").transform);

        // DROP COLLECTABLE
        if (Random.Range(0, 100) <= dropProbability)
            Instantiate(collectables[Random.Range(0, collectables.Count)], collectableSpawnPoint.position, Quaternion.Euler(Vector3.zero), GameObject.Find("--- COLLECTABLES ---").transform);
        
        // DESTROY COMPONENT
        Destroy(this);
    }
}
