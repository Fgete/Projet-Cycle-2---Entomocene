using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableFurniture : MonoBehaviour
{
    [Header("DROPS")]
    [Range(0, 100)]
    public int dropProbability;
    public List<GameObject> collectables;

    [Header("SOUNDS")]
    public AudioSource breakSound;
    public List<AudioClip> breakClips;

    [Header("VFX")]
    public ParticleSystem particles;
    public List<GameObject> scraps;

    public void Break()
    {
        // PLAY SOUND
        if (breakClips.Count > 0)
        {
            breakSound.clip = breakClips[Random.Range(0, breakClips.Count)];
            breakSound.Play();
        }

        // PARTICLE IN VFX PARENT
        if (particles)
            Instantiate(particles, transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0, Random.Range(0, 360), 0), GameObject.Find("--- VFX ---").transform);

        // DROP COLLECTABLE
        if (Random.Range(0, 100) <= dropProbability)
            Instantiate(collectables[Random.Range(0, collectables.Count)], transform.position, Quaternion.Euler(Vector3.zero), GameObject.Find("--- COLLECTABLES ---").transform);

        // DISABLE MESH
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        foreach (GameObject s in scraps)
            s.SetActive(true);

        // DESTROY WHEN AUDIO ENDS
        StartCoroutine(WaitSoundEnd());
    }

    IEnumerator WaitSoundEnd()
    {
        yield return new WaitWhile(() => breakSound.isPlaying);
        Destroy(transform.parent.gameObject);
    }
}
