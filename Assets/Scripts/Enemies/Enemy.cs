using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum DamageType { normal, threwArmor, onlyArmor }

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float life;
    public float speed;
    public float damage;
    public DamageType damageType;
    [Range(0, 100)]
    public int damageAbsorption;
    [Space]
    public float walkDistance;
    // public float attackDistance;
    public float stopDistance;
    public Transform attackArea;
    public float attackRadius;
    [HideInInspector]
    public float attackBackDistance = 2.5f;
    [HideInInspector]
    public bool alive = false;

    [Header("Sounds")]
    public AudioSource walkAudioSource;
    public AudioSource hitAudioSource;
    public AudioSource biteAudioSource;
    public AudioSource deathAudioSource;
    public List<AudioClip> hit;
    public List<AudioClip> bite;
    public List<AudioClip> death;
    private Music_Manager mm;

    [Header("VFX")]
    public float particleHeight;
    public ParticleSystem blood;
    public int deathParticleNumber;
    public float destroyTime;
    public List<GameObject> fleshPieces;

    [Header("Externals")]
    private Transform player;
    private NavMeshAgent nma;
    private BoxCollider selfCollider;
    private Transform particleParent;

    [Header("Animation")]
    private Animator ac;


    private void Awake()
    {
        mm             = FindObjectOfType<Music_Manager>();
        player         = FindObjectOfType<CharacterController>().transform;
        nma            = transform.parent.GetComponent<NavMeshAgent>();
        ac             = transform.GetComponentInChildren<Animator>();
        selfCollider   = transform.parent.GetComponent<BoxCollider>();
        particleParent = GameObject.Find("--- VFX ---").transform;
    }

    private void Update()
    {
        // Verify if player is in range
        if (Vector3.Distance(transform.position, player.position) > walkDistance)
        {
            if (HasParameter("isWalking", ac))
                if (ac.GetBool("isWalking"))
                    ac.SetBool("isWalking", false);
            if (walkAudioSource.isPlaying)
                walkAudioSource.Stop();
            return;
        }

        // Verify if nav mesh agent is enabled
        if (!nma.enabled)
            return;
        
        // Verify if the player still alive
        if (!alive)
            return;

        // Set distances
        float distanceToTarget = 0;
        distanceToTarget = Vector3.Distance(nma.destination, transform.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Detection & set destination
        if (distanceToPlayer < walkDistance && distanceToPlayer > attackRadius * attackBackDistance)
            if (Vector3.Distance(player.position, nma.destination) > attackRadius)
            {
                nma.SetDestination(player.position);
                if (mm.action <= 10)
                    mm.GetAction();
            }

        // Set orientation to player
        if (distanceToPlayer < attackRadius * attackBackDistance + .5f)
        {
            transform.parent.LookAt(player);
            if (mm.action <= 10)
                mm.GetAction();
        }

        // Attack & Walk animations
        if (HasParameter("isAttacking", ac))
            ac.SetBool("isAttacking", distanceToPlayer < attackRadius * attackBackDistance + .5f && player.GetComponent<Character_Stats>().alive);
        if (HasParameter("isWalking", ac))
            ac.SetBool("isWalking", distanceToTarget > stopDistance);

        // Walking sound
        if (HasParameter("isWalking", ac))
        {
            if (ac.GetBool("isWalking") && !walkAudioSource.isPlaying)
                walkAudioSource.Play();
            else if (!ac.GetBool("isWalking") && walkAudioSource.isPlaying)
                walkAudioSource.Stop();
        }

    }

    private void EnemyAttack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(attackArea.position, attackRadius);
        foreach (var hitCollider in hitColliders)
            if (hitCollider.CompareTag("Player") && life > 0)
            {
                player.GetComponent<Character_Stats>().GetDamage(damage, damageType);
                biteAudioSource.clip = bite[Random.Range(0, bite.Count)];
                biteAudioSource.Play();
            }
    }

    public void EnemyGetDamage(float nDamage)
    {
        // --- AI ---
        if (Vector3.Distance(transform.position, player.position) > walkDistance)
            nma.SetDestination(player.position);
        
        // --- LIFE ---
        life -= (1 - damageAbsorption / 100) * nDamage;

        // --- DEATH ---
        if (life <= 0)
            EnemyDeath();
        else
        {
            // --- AUDIO ---
            if (hit.Count > 0)
                hitAudioSource.clip = hit[Random.Range(0, hit.Count)];
            if (hitAudioSource.clip)
                hitAudioSource.Play();
        }
    }

    public void EnemyDeath()
    {
        // --- STATE ---
        alive = false;

        // --- AUDIOS ---
        if (death.Count > 0)
            deathAudioSource.clip = death[Random.Range(0, death.Count)];
        if (deathAudioSource.clip)
            deathAudioSource.Play();
        walkAudioSource.Stop();

        // --- AI AGENT ---
        nma.enabled = false;

        // --- COLLIDER ---
        selfCollider.enabled = false;

        // --- DESTROY ---
        // Destroy main mesh
        foreach (SkinnedMeshRenderer smr in transform.GetComponentsInChildren<SkinnedMeshRenderer>())
            smr.enabled = false;
        // Spawn particles
        for (int i = 0; i < deathParticleNumber; i++)
            Instantiate(blood, transform.position, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), particleParent);
        // Appear flesh meshs
        foreach (GameObject fp in fleshPieces)
            fp.SetActive(true);
        // Desapear flesh meshs smoothly DONE

        // Wait time & destroy
        StartCoroutine(WaitForDestroy());
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitWhile(() => deathAudioSource.isPlaying);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, particleHeight, transform.position.z), .1f);
        Gizmos.DrawWireSphere(attackArea.position, attackRadius);
    }
    
    private bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
            if (param.name == paramName)
                return true;
        return false;
    }
}
