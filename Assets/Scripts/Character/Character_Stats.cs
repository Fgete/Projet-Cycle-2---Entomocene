using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character_Stats : MonoBehaviour
{
    [Header("Character")]
    [Range(0, 100)]
    public float life;
    [Range(0, 100)]
    public float armor;
    [Range(.5f, 1.5f)]
    public float speed;
    public float slapDamage;
    [HideInInspector]
    public bool alive = true;

    [Header("Sounds")]
    private Music_Manager mm;

    public AudioSource characterDamageSound;
    public List<AudioClip> damageClips;
    public List<AudioClip> bloodClips;
    public List<AudioClip> acidClips;
    [Space]
    public AudioSource characterLifeSound;
    public List<AudioClip> lifeClips;
    public AudioSource characterArmorSound;
    public List<AudioClip> armorClips;
    public AudioSource characterAmmoSound;
    public List<AudioClip> ammoClips;
    [Space]
    public AudioSource weaponShotSound;
    public AudioSource weaponReloadSound;
    public List<AudioClip> weaponShotClips;
    public AudioClip weaponEmpty;
    public AudioClip weaponReload;
    [Space]
    public AudioSource slapSound;
    public List<AudioClip> slapClips;
    public List<AudioClip> slapFleshClips;
    [Space]
    public AudioSource characterDeathSound;

    [Header("Weapon")]
    public float weaponDamage;
    public uint shots = 1;
    public float shotDispertion;
    public float weaponCooldown;
    [Range(0, 17)]
    public int weaponMagasin;
    [Range(0, 256)]
    public int weaponAmmo;
    public bool weaponInfiniteAmmo = false;

    [Header("HUD")]
    private UI_HUD hud;
    public Image uiLife;
    public Image uiArmor;
    public Text uiAmmo;

    private Character_Death cd;

    private void Awake()
    {
        UpdateHud();

        mm = FindObjectOfType<Music_Manager>();
        hud = FindObjectOfType<UI_HUD>();
        cd = GetComponent<Character_Death>();
    }

    public bool WeaponShot()
    {
        if (weaponMagasin > 0)
        {
            weaponMagasin--;
            weaponShotSound.clip = weaponShotClips[Random.Range(0, weaponShotClips.Count)];
            weaponShotSound.Play();
            UpdateHud();
            return true;
        }
        weaponShotSound.clip = weaponEmpty;
        weaponShotSound.Play();
        return false;
    }

    public void WeaponReload()
    {
        if (weaponMagasin < 17)
        {
            if (weaponInfiniteAmmo)
            {
                weaponMagasin = 17;
                weaponReloadSound.clip = weaponReload;
                weaponReloadSound.Play();
            }
            else
            {
                if (weaponAmmo > (17 - weaponMagasin))
                {
                    weaponAmmo -= (17 - weaponMagasin);
                    weaponMagasin = 17;
                    weaponReloadSound.clip = weaponReload;
                    weaponReloadSound.Play();
                }
                else if (weaponAmmo > 0)
                {
                    weaponMagasin += weaponAmmo;
                    weaponAmmo = 0;
                    weaponReloadSound.clip = weaponReload;
                    weaponReloadSound.Play();
                }
            }
            UpdateHud();
            hud.Ammo_Feedback();
        }
    }

    public void Slap(bool enemyTouched)
    {
        if (enemyTouched)
        {
            slapSound.clip = slapFleshClips[Random.Range(0, slapFleshClips.Count)];
            slapSound.Play();
        }
        else
        {
            slapSound.clip = slapClips[Random.Range(0, slapClips.Count)];
            slapSound.Play();
        }
    }

    public void GetDamage(float d , DamageType dt)
    {
        if (alive)
        {
            mm.GetDamage();

            switch (dt)
            {
                case DamageType.normal:
                    float damageToArmor = d * armor / 100;
                    float damageToLife = d - damageToArmor;
                    armor -= damageToArmor;
                    life -= damageToLife;
                    if (damageClips.Count > 0)
                    {
                        characterDamageSound.clip = damageClips[Random.Range(0, damageClips.Count)];
                        characterDamageSound.Play();
                    }
                    break;
                case DamageType.onlyArmor:
                    armor -= d;
                    characterDamageSound.clip = acidClips[Random.Range(0, acidClips.Count)];
                    characterDamageSound.Play();
                    break;
                case DamageType.threwArmor:
                    life -= d;
                    characterDamageSound.clip = bloodClips[Random.Range(0, bloodClips.Count)];
                    characterDamageSound.Play();
                    break;
            }
            UpdateHud();
            hud.Damage_Feedback();

            if (life <= 0 && alive)
                Death();
        }
        
    }

    public void GetLife(float l)
    {
        if (life + l > 100)
            life = 100;
        else
            life += l;
        PlayLifeClip();
        UpdateHud();
        hud.Life_Feedback();
    }

    public void GetArmor(float a)
    {
        if (armor + a > 100)
            armor = 100;
        else
            armor += a;
        PlayArmorClip();
        UpdateHud();
        hud.Armor_Feedback();
    }

    public void GetAmmo(int a)
    {
        if (weaponAmmo + a > 256)
            weaponAmmo = 256;
        else
            weaponAmmo += a;
        PlayAmmoClip();
        UpdateHud();
        hud.Ammo_Feedback();
    }

    public void UpdateHud()
    {
        uiLife.fillAmount = life / 100;
        uiArmor.fillAmount = armor / 100;
        if (weaponInfiniteAmmo)
            uiAmmo.text = weaponMagasin.ToString() + " | ∞";
        else
            uiAmmo.text = weaponMagasin.ToString() + " | " + weaponAmmo.ToString();
    }

    private void Death()
    {
        alive = false;
        cd.Run();
    }

    private void PlayLifeClip()
    {
        characterLifeSound.clip = lifeClips[Random.Range(0, lifeClips.Count)];
        characterLifeSound.Play();
    }

    private void PlayArmorClip()
    {
        characterArmorSound.clip = armorClips[Random.Range(0, armorClips.Count)];
        characterArmorSound.Play();
    }

    private void PlayAmmoClip()
    {
        characterAmmoSound.clip = ammoClips[Random.Range(0, ammoClips.Count)];
        characterAmmoSound.Play();
    }
}
