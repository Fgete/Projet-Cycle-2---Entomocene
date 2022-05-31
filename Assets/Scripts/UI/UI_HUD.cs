using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HUD : MonoBehaviour
{
    [Header("Stats")]
    public float feedbackFadeTime;

    [Header("Colors")]
    public Color imageColor;
    public Color textColor;
    public Color bloodColor;
    public Color armorColor;
    public Color ammoColor;

    [Header("Life Elements")]
    public Image lifeGauge;
    public List<Image> lifeImages;
    public List<Text> lifeTexts;

    [Header("Armor Elements")]
    public Image armorGauge;
    public List<Image> armorImages;
    public List<Text> armorTexts;

    [Header("Ammo Elements")]
    public List<Image> ammoImages;
    public List<Text> ammoTexts;

    public void Damage_Feedback()
    {
        StopAllCoroutines();
        StartCoroutine(Damage_Evolution());
    }

    public void Ammo_Feedback()
    {
        // StopAllCoroutines();
        StartCoroutine(Ammo_Evolution());
    }

    public void Armor_Feedback()
    {
        // StopAllCoroutines();
        StartCoroutine(Armor_Evolution());
    }

    public void Life_Feedback()
    {
        // StopAllCoroutines();
        StartCoroutine(Life_Evolution());
    }

    IEnumerator Damage_Evolution()
    {
        lifeGauge.color = Color.white;
        armorGauge.color = Color.white;
        SetColor(lifeImages, lifeTexts, bloodColor, bloodColor);
        SetColor(armorImages, armorTexts, bloodColor, bloodColor);
        SetColor(ammoImages, ammoTexts, bloodColor, bloodColor);
        for (int i = 0; i <= 100; i++)
        {
            Color newLifeGaugeColor = new Color(
                Color.white.r * (1 - (float)i / 100) + bloodColor.r * ((float)i / 100),
                Color.white.g * (1 - (float)i / 100) + bloodColor.g * ((float)i / 100),
                Color.white.b * (1 - (float)i / 100) + bloodColor.b * ((float)i / 100),
                Color.white.a * (1 - (float)i / 100) + bloodColor.a * ((float)i / 100)
                );
            Color newArmorGaugeColor = new Color(
                Color.white.r * (1 - (float)i / 100) + armorColor.r * ((float)i / 100),
                Color.white.g * (1 - (float)i / 100) + armorColor.g * ((float)i / 100),
                Color.white.b * (1 - (float)i / 100) + armorColor.b * ((float)i / 100),
                Color.white.a * (1 - (float)i / 100) + armorColor.a * ((float)i / 100)
                );
            Color newImageColor = new Color(
                bloodColor.r * (1 - (float)i / 100) + imageColor.r * ((float)i / 100),
                bloodColor.g * (1 - (float)i / 100) + imageColor.g * ((float)i / 100),
                bloodColor.b * (1 - (float)i / 100) + imageColor.b * ((float)i / 100),
                bloodColor.a * (1 - (float)i / 100) + imageColor.a * ((float)i / 100)
                );
            Color newTextColor = new Color(
                bloodColor.r * (1 - (float)i / 100) + textColor.r * ((float)i / 100),
                bloodColor.g * (1 - (float)i / 100) + textColor.g * ((float)i / 100),
                bloodColor.b * (1 - (float)i / 100) + textColor.b * ((float)i / 100),
                bloodColor.a * (1 - (float)i / 100) + textColor.a * ((float)i / 100)
                );
            lifeGauge.color = newLifeGaugeColor;
            armorGauge.color = newArmorGaugeColor;
            SetColor(lifeImages, lifeTexts, newImageColor, newTextColor);
            SetColor(armorImages, armorTexts, newImageColor, newTextColor);
            SetColor(ammoImages, ammoTexts, newImageColor, newTextColor);
            yield return new WaitForSeconds(feedbackFadeTime);
        }
    }
    
    IEnumerator Ammo_Evolution()
    {
        SetColor(ammoImages, ammoTexts, ammoColor, ammoColor);
        for (int i = 0; i <= 100; i++)
        {
            Color newImageColor = new Color(
                ammoColor.r * (1 - (float)i / 100) + imageColor.r * ((float)i / 100),
                ammoColor.g * (1 - (float)i / 100) + imageColor.g * ((float)i / 100),
                ammoColor.b * (1 - (float)i / 100) + imageColor.b * ((float)i / 100),
                ammoColor.a * (1 - (float)i / 100) + imageColor.a * ((float)i / 100)
                );
            Color newTextColor = new Color(
                ammoColor.r * (1 - (float)i / 100) + textColor.r * ((float)i / 100),
                ammoColor.g * (1 - (float)i / 100) + textColor.g * ((float)i / 100),
                ammoColor.b * (1 - (float)i / 100) + textColor.b * ((float)i / 100),
                ammoColor.a * (1 - (float)i / 100) + textColor.a * ((float)i / 100)
                );
            SetColor(ammoImages, ammoTexts, newImageColor, newTextColor);
            yield return new WaitForSeconds(feedbackFadeTime);
        }
    }

    IEnumerator Armor_Evolution()
    {
        armorGauge.color = Color.white;
        SetColor(armorImages, armorTexts, armorColor, armorColor);
        for (int i = 0; i <= 100; i++)
        {
            Color newGaugeColor = new Color(
                Color.white.r * (1 - (float)i / 100) + armorColor.r * ((float)i / 100),
                Color.white.g * (1 - (float)i / 100) + armorColor.g * ((float)i / 100),
                Color.white.b * (1 - (float)i / 100) + armorColor.b * ((float)i / 100),
                Color.white.a * (1 - (float)i / 100) + armorColor.a * ((float)i / 100)
                );
            Color newImageColor = new Color(
                armorColor.r * (1 - (float)i / 100) + imageColor.r * ((float)i / 100),
                armorColor.g * (1 - (float)i / 100) + imageColor.g * ((float)i / 100),
                armorColor.b * (1 - (float)i / 100) + imageColor.b * ((float)i / 100),
                armorColor.a * (1 - (float)i / 100) + imageColor.a * ((float)i / 100)
                );
            Color newTextColor = new Color(
                armorColor.r * (1 - (float)i / 100) + textColor.r * ((float)i / 100),
                armorColor.g * (1 - (float)i / 100) + textColor.g * ((float)i / 100),
                armorColor.b * (1 - (float)i / 100) + textColor.b * ((float)i / 100),
                armorColor.a * (1 - (float)i / 100) + textColor.a * ((float)i / 100)
                );
            armorGauge.color = newGaugeColor;
            SetColor(armorImages, armorTexts, newImageColor, newTextColor);
            yield return new WaitForSeconds(feedbackFadeTime);
        }
    }

    IEnumerator Life_Evolution()
    {
        lifeGauge.color = Color.white;
        SetColor(lifeImages, lifeTexts, bloodColor, bloodColor);
        for (int i = 0; i <= 100; i++)
        {
            Color newGaugeColor = new Color(
                Color.white.r * (1 - (float)i / 100) + bloodColor.r * ((float)i / 100),
                Color.white.g * (1 - (float)i / 100) + bloodColor.g * ((float)i / 100),
                Color.white.b * (1 - (float)i / 100) + bloodColor.b * ((float)i / 100),
                Color.white.a * (1 - (float)i / 100) + bloodColor.a * ((float)i / 100)
                );
            Color newImageColor = new Color(
                bloodColor.r * (1 - (float)i / 100) + imageColor.r * ((float)i / 100),
                bloodColor.g * (1 - (float)i / 100) + imageColor.g * ((float)i / 100),
                bloodColor.b * (1 - (float)i / 100) + imageColor.b * ((float)i / 100),
                bloodColor.a * (1 - (float)i / 100) + imageColor.a * ((float)i / 100)
                );
            Color newTextColor = new Color(
                bloodColor.r * (1 - (float)i / 100) + textColor.r * ((float)i / 100),
                bloodColor.g * (1 - (float)i / 100) + textColor.g * ((float)i / 100),
                bloodColor.b * (1 - (float)i / 100) + textColor.b * ((float)i / 100),
                bloodColor.a * (1 - (float)i / 100) + textColor.a * ((float)i / 100)
                );
            lifeGauge.color = newGaugeColor;
            SetColor(lifeImages, lifeTexts, newImageColor, newTextColor);
            yield return new WaitForSeconds(feedbackFadeTime);
        }
    }

    private void SetColor(List<Image> images, List<Text> texts, Color imageColor, Color textColor)
    {
        foreach (Image i in images)
            i.color = imageColor;

        foreach (Text t in texts)
            t.color = textColor;
    }
}
