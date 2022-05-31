using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Variations : MonoBehaviour
{
    [Header("Variation")]
    public float minimumVariation;
    public float maximumVariation;
    private float variation;

    [Header("Color")]
    public List<Color> colors = new List<Color>();

    [Header("Renderers")]
    public List<SkinnedMeshRenderer> skins;
    public List<MeshRenderer> renderers;

    [Header("Audios")]
    public List<AudioSource> audioSources;

    void Start()
    {
        Enemy e = GetComponent<Enemy>();
        NavMeshAgent nma = transform.parent.GetComponent<NavMeshAgent>();

        if (maximumVariation > minimumVariation)
        {
            variation = Random.Range(minimumVariation, maximumVariation);

            // Set stats variations
            e.life               *= Mathf.Pow(variation, 2);
            e.speed              /= variation;
            e.damage             *= variation;
            e.attackRadius       *= variation;
            transform.localScale *= variation;

            // Set movement variations to NavMeshAgent
            nma.speed = e.speed;
            nma.stoppingDistance = e.attackRadius * e.attackBackDistance; // Depend of the attack distance in Enemy component

            // Set color
            if (colors.Count > 0)
            {
                Color color = colors[Random.Range(0, colors.Count)];
                foreach (MeshRenderer mr in renderers)
                    mr.material.color = color;
                foreach (SkinnedMeshRenderer smr in skins)
                    smr.material.color = color;
            }

            // Pitch audios
            foreach (AudioSource audio in audioSources)
                audio.pitch = Mathf.Pow(variation, 2);

            // Set animation speed
            GetComponent<Animator>().speed /= variation;
        }
    }
}
