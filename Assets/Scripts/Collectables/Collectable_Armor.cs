using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Armor : MonoBehaviour
{
    public int maxArmor;
    public float velocity;
    public ParticleSystem particles;

    private void Start()
    {
        Transform particleParent = GameObject.Find("--- VFX ---").transform;
        Instantiate(particles, transform.position, Quaternion.Euler(-90, 0, 0), particleParent);
    }
    
    private void Update()
    {
        transform.eulerAngles += new Vector3(0, Time.deltaTime * velocity, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Character_Stats>().GetArmor(Random.Range(1, maxArmor));
            Destroy(gameObject);
        }
    }
}
