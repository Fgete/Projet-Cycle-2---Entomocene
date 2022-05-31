using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_FadeIn : MonoBehaviour
{
    public List<Image> images;
    public List<Text> texts;
    [Space]
    public float fade;

    private float alpha = 0f;
    private levelEnum nextLevel;

    private void Start()
    {
        if (FindObjectOfType<Map_Generation_Manager>())
            nextLevel = FindObjectOfType<Map_Generation_Manager>().nextScene;
        else
            nextLevel = levelEnum.FirstLevel;
        foreach (Image i in images)
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        foreach (Text t in texts)
            t.color = new Color(t.color.r, t.color.g, t.color.b, 0);
        gameObject.SetActive(false);
    }

    public void Run()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (alpha < 1)
        {
            alpha += Time.deltaTime * fade;
            yield return new WaitForSeconds(Time.deltaTime);
            foreach (Image i in images)
                i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
            foreach (Text t in texts)
                t.color = new Color(t.color.r, t.color.g, t.color.b, alpha);
        }
        SceneManager.LoadScene(nextLevel.ToString());
        yield return null;
    }
}
