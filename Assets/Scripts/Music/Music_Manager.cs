using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music_Manager : MonoBehaviour
{
    [Header("Main")]
    [Range(0, 1)]
    public float mainVolume = 1;
    [Header("Neutral")]
    public float neutre     = 1f;
    [Header("Action")]
    public bool actionSet   = true;
    public float actionUp   = .1f;
    public float actionFade = 1f;
    public float action     = 0f;
    [Header("Damage")]
    public float damageUp   = .5f;
    public float damageFade = 1f;
    public float damage     = 0f;
    [Header("Melancoly")]
    public bool melancSet   = true;
    public float melancFade = 1f;
    public float melanc     = 0f;

    [Header("Tracks")]
    public AudioSource neutreTrack;
    public AudioSource actionTrack;
    public AudioSource damageTrack;
    public AudioSource melancTrack;

    private float damageTemp = 0;
    private float actionTemp = 0;
    private float melancTemp = 0;

    private float GetAllWeights()
    {
        return neutre + (action * BoolToInt(actionSet)) + damage + (melanc * BoolToInt(melancSet));
    }

    private void Update()
    {
        if (damage > 0)              { damage -= damage * damageFade * Time.deltaTime + .0001f; }
        if (action > 0 && actionSet) { action -= action * actionFade * Time.deltaTime + .0001f; }
        if (melanc > 0 && melancSet) { melanc -= melanc * melancFade * Time.deltaTime + .0001f; }

        if (damageTemp != damage)
        {
            damageTrack.volume = damage * mainVolume / GetAllWeights();
            damageTemp = damage;
        }
        if (actionTemp != action && actionSet)
        {
            actionTrack.volume = action * mainVolume / GetAllWeights();
            actionTemp = action;
        }
        if (melancTemp != melanc && melancSet)
        {
            melancTrack.volume = melanc * mainVolume / GetAllWeights();
            melancTemp = melanc;
        }

        neutreTrack.volume = neutre * mainVolume / GetAllWeights();
    }

    public void GetDamage()
    {
        damage += damageUp;
    }

    public void GetAction()
    {
        if (actionSet)
            action += actionUp;
    }

    public int BoolToInt(bool b)
    {
        if (b)
            return 1;
        else
            return 0;
    }
}
