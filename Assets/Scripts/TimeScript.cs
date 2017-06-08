using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{

    public float timePerHit = 0.4f;
    private float timeElapsed = 0;
    private float dificultyFactor;
    public float initialDificulty = 500;
    public float initialTime = 15;
    private float time;
    private Image timeBar;
    private bool started = false;
    public int countDownTime = 3;
    public GameObject messagePrefab;

    public GameObject circlePrefab;

    private static string MESSAGE_GO = "GO";

    void Start()
    {
        readDifficulty();
        timeBar = GetComponent<Image>();
        StartCoroutine(CountDown(countDownTime));
    }

    private void readDifficulty()
    {
        string difficulty = PlayerPrefs.GetString("Difficulty");
        switch (difficulty)
        {
            case "Easy":
                timePerHit = 0.4f;
                break;
            case "Medium":
                timePerHit = 0.3f;
                break;
            case "Hard":
                timePerHit = 0.2f;
                break;
            default:
                break;
        }
        Debug.Log("Difficulty loaded: " + difficulty);
    }

    void Begin()
    {
        FindObjectOfType<OnePlayerScript>().startGame();
        started = true;
        time = initialTime;
        dificultyFactor = initialDificulty;
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timeBar.fillAmount = time / initialTime;
                timeElapsed += Time.deltaTime;
            }
            else
            {
                started = false;
                FindObjectOfType<OnePlayerScript>().GameOver();
            }
        }
    }


    // Use this for initialization
    IEnumerator CountDown(int count)
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = countDownTime; i > 0; i--)
        {
            showTime(i.ToString());
            yield return new WaitForSeconds(1.0f);
        }
        showTime(MESSAGE_GO);
        yield return new WaitForSeconds(1.0f);
        Begin();
    }

    void showTime(string text)
    {
        GameObject g = (GameObject)Instantiate(messagePrefab, Vector3.zero, Quaternion.identity);
        g.transform.SetParent(FindObjectOfType<Canvas>().transform);
        g.transform.localScale = Vector3.one;
        g.GetComponent<Text>().text = text;
        g.GetComponent<MessageScript>().FadeOut();
    }

    public void extraSeconds(int bestSolution, int solution)
    {
        float plusTouches = ((float)bestSolution / solution);
        float plusVelocity = (timePerHit * solution / timeElapsed);
        plusVelocity = Mathf.Max(Mathf.Min(plusVelocity, 1.2f), 0);
        float plus = (plusTouches * plusVelocity);
        time += plus;
        time = Mathf.Min(time, initialTime);
        timeElapsed = 0;
    }

}
