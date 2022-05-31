using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Action : MonoBehaviour
{
    [Header("Body")]
    public Animator topBodyAnimator;

    [Header("Weapons")]
    public GameObject gunMesh;
    public GameObject swordMesh;
    public Transform bulletOutSpot;
    public Animator gunFireAnimator;
    public GameObject flashlight;
    [Space]
    public GameObject shotLinePrefab;
    private bool fireButtonPressed = false;
    private bool canFire = true;
    public KeyCode reloadKey = KeyCode.R;
    private bool reloadButtonPressed = false;
    public KeyCode slapKey = KeyCode.Space;
    private bool slapButtonPressed = false;
    [HideInInspector]
    public bool isSlapping = false;

    [Header("VFX")]
    public ParticleSystem bulletImpact;
    
    private Character_Stats cs;
    private Transform particleParent;
    private float shotTime, shotTemp = 0;


    private void Awake()
    {
        cs = GetComponent<Character_Stats>();
        particleParent = GameObject.Find("--- VFX ---").transform;

        swordMesh.SetActive(false);
    }

    private void Update()
    {
        // --- FIRE ---
        if (Input.GetButtonDown("Fire1") && !fireButtonPressed && !isSlapping && canFire && shotTime > cs.weaponCooldown + shotTemp)
        {
            shotTemp = shotTime;
            fireButtonPressed = true;
            if (cs.WeaponShot())
                for (int i = 0; i < cs.shots; i++)
                    GunShot();
        }

        if (Input.GetButtonUp("Fire1"))
            fireButtonPressed = false;

        // --- RELOAD ---
        if (Input.GetKeyDown(reloadKey) && !reloadButtonPressed)
        {
            reloadButtonPressed = true;
            Reload();
        }

        if (Input.GetKeyUp(reloadKey))
            reloadButtonPressed = false;

        // --- SLAP ---
        if (Input.GetKeyDown(slapKey) && !slapButtonPressed)
        {
            slapButtonPressed = true;
            topBodyAnimator.SetTrigger("Slap");
        }

        if (Input.GetKeyUp(slapKey))
            slapButtonPressed = false;

        // --- DETECT WALL COLLISION ---
        Collider[] colliders = Physics.OverlapSphere(bulletOutSpot.position, .3f);
        canFire = true;
        if (colliders.Length != 0)
            foreach (Collider collider in colliders)
                if (collider.tag == "Wall")
                    canFire = false;

        // --- FLASHLIGHT ---
        flashlight.GetComponent<Light>().enabled = canFire;

        // --- TIMERS ---
        RunTimers(); // Increment all timers
    }

    public void GunShot()
    {
        // --- RAYCAST SCOPE ---
        RaycastHit hit;
        // Vector3 dispertionVector = new Vector3(0, 0, Random.Range(cs.shotDispertion * cs.shots, cs.shotDispertion * cs.shots));
        // Vector3 rayDirection = bulletOutSpot.forward + dispertionVector;
        Ray ray = new Ray(bulletOutSpot.position, bulletOutSpot.forward);
        int layer_mask0 = LayerMask.GetMask("Default");
        int layer_mask1 = LayerMask.GetMask("Ground");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask0, QueryTriggerInteraction.Ignore) || Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask1, QueryTriggerInteraction.Ignore))
        {
            // DRAW SHOT LINE
            GameObject line = Instantiate(shotLinePrefab, bulletOutSpot.position, bulletOutSpot.rotation, particleParent);
            line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, hit.distance));

            // TOUCHED OBJ
            GameObject col = hit.collider.gameObject;
            if (col.tag == "Enemy")
            {
                if (col.GetComponentInChildren<Enemy>())
                {
                    Enemy e = col.GetComponentInChildren<Enemy>();
                    e.EnemyGetDamage(cs.weaponDamage);
                    Instantiate(
                        e.blood,
                        new Vector3(hit.point.x, e.particleHeight, hit.point.z),
                        Quaternion.LookRotation(bulletOutSpot.position - hit.point, bulletOutSpot.position),
                        particleParent
                    );
                }
            }
            else
                Instantiate(bulletImpact, hit.point, Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), particleParent);

            // TOP BODY ANIMATION
            topBodyAnimator.SetTrigger("Shot");

            // GUN ANIMATION
            gunFireAnimator.SetTrigger("Shot");
        }
    }

    public void Reload()
    {
        cs.WeaponReload();
    }

    private void OnDrawGizmos()
    {
        /*
        if (bulletOutSpot)
        {
            Gizmos.color = Color.yellow;
            RaycastHit hit;
            Vector3 dispertionVector = new Vector3(cs.shotDispertion * cs.shots, cs.shotDispertion * cs.shots, 0);
            Vector3 rayDirectionMin = bulletOutSpot.forward - dispertionVector;
            Vector3 rayDirectionMax = bulletOutSpot.forward + dispertionVector;
            Ray rayMin = new Ray(bulletOutSpot.position, rayDirectionMin);
            Ray rayMax = new Ray(bulletOutSpot.position, rayDirectionMax);
            int layer_mask = LayerMask.GetMask("Default");
            if (Physics.Raycast(rayMin, out hit, Mathf.Infinity, layer_mask, QueryTriggerInteraction.Ignore))
                Gizmos.DrawLine(bulletOutSpot.position, hit.point);
            if (Physics.Raycast(rayMax, out hit, Mathf.Infinity, layer_mask, QueryTriggerInteraction.Ignore))
                Gizmos.DrawLine(bulletOutSpot.position, hit.point);
        }
        */

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bulletOutSpot.position, .3f);
    }

    private void RunTimers()
    {
        shotTime += Time.deltaTime;
    }
}
