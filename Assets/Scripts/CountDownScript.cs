using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownScript : MonoBehaviour
{

    public int initialTime;

    private int time;

    private Text timeText;

    // Use this for initialization
    void Start()
    {
        timeText = GetComponent<Text>();
        time = initialTime;
        InvokeRepeating("StartCountDown", 0, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = timeText.color.a - (Time.deltaTime * 0.5f);
        Debug.Log("DeltaTime: " + Time.deltaTime);
        Color temp = new Color(timeText.color.r, timeText.color.g, timeText.color.b, alpha);
        timeText.color = temp;
    }

    private void decreaseAlpha()
    {
        while (timeText.color.a > 0)
        {
            float alpha = timeText.color.a - 0.00000000001f;
            Debug.Log("alpha: " + alpha);
            Color temp = new Color(timeText.color.r, timeText.color.g, timeText.color.b, alpha);
            timeText.color = temp;
        }
    }

    private void resetAlpha()
    {
        Color temp = new Color(timeText.color.r, timeText.color.g, timeText.color.b, 1.0f);
        timeText.color = temp;
    }

    private void StartCountDown()
    {
        resetAlpha();
        if (time > 0)
        {
            timeText.text = time.ToString();
            time--;
            decreaseAlpha();
        }
        else
        {
            timeText.text = "GO";
            CancelInvoke("StartCountDown");
            decreaseAlpha();
            gameObject.SetActive(false);
        }

    }
}
