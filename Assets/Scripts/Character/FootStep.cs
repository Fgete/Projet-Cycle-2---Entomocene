using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public List<AudioClip> stepClips;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        audioSource.clip = stepClips[Random.Range(0, stepClips.Count)];
        audioSource.Play();
        Debug.Log("PLAY");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, GetComponent<SphereCollider>().radius);
    }
}
