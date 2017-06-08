using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour
{

    public float fadeSpeed = 1.5f;

    public float alpha = 0.5f;

    private bool fadeOut = false;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            if (alpha > 0)
            {
                Text text = GetComponent<Text>();
                alpha -= fadeSpeed * Time.deltaTime;
                alpha = Mathf.Clamp01(alpha);
                text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            }
            else
            {
				Destroy(gameObject);
            }
        }
    }

    public void FadeOut()
    {
        fadeOut = true;
    }
}
