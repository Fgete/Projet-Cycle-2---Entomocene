using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeOut : MonoBehaviour
{
    public List<Image> images;
    public List<Text> texts;
    public Text feedback;
    public Color feedbackColor;
    [Space]
    public float fade;
    public bool run = false;

    private float alpha = 1f;

    private void Update()
    {
        if (run)
        {
            if (feedback)
            {
                feedback.text = "- READY -";
                feedback.color = feedbackColor;
            }
            alpha -= Time.deltaTime * fade;

            if (alpha > 0)
            {
                foreach (Image i in images)
                    i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
                foreach (Text t in texts)
                    t.color = new Color(t.color.r, t.color.g, t.color.b, alpha);
            }
            else
                gameObject.SetActive(false);
        }
    }
}
