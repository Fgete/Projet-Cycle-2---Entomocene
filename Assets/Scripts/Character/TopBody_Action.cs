using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBody_Action : MonoBehaviour
{
    public GameObject gunMesh;
    public GameObject swordMesh;
    public Transform slapArea;

    private Character_Stats cs;
    private Character_Action ca;
    private Transform particleParent;

    private void Awake()
    {
        cs = FindObjectOfType<Character_Stats>();
        ca = FindObjectOfType<Character_Action>();
        particleParent = GameObject.Find("--- VFX ---").transform;
    }

    public void SlapEvent()
    {
        // --- PLAY DEFAULT SOUND ---
        cs.Slap(false);

        // --- TOUCHED OBJ ---
        Collider[] hitColliders = Physics.OverlapCapsule(
            new Vector3(slapArea.transform.position.x, 0, slapArea.transform.position.z),
            new Vector3(slapArea.transform.position.x, 2, slapArea.transform.position.z),
            .5f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                cs.Slap(true);
                Enemy e = hitCollider.transform.GetComponentInChildren<Enemy>();
                e.EnemyGetDamage(cs.slapDamage);
                Instantiate(
                    e.blood,
                    swordMesh.transform.position,
                    Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)),
                    particleParent
                );
            }
            else if (hitCollider.tag == "Door")
            {
            }
            else if (hitCollider.tag == "Breakable")
            {
                BreakableFurniture bf = hitCollider.GetComponent<BreakableFurniture>();
                ActivatableFurniture af = hitCollider.GetComponent<ActivatableFurniture>();
                if (bf)
                    bf.Break();
                else if (af)
                    af.Activate();
            }
        }            
    }

    public void SlapPlay()
    {
        gunMesh.SetActive(false);
        swordMesh.SetActive(true);

        ca.isSlapping = true;
    }

    public void SlapStop()
    {
        gunMesh.SetActive(true);
        swordMesh.SetActive(false);

        ca.isSlapping = false;
    }
}
