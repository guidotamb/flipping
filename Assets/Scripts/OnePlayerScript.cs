using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnePlayerScript : MonoBehaviour
{
    private bool inGame;
    private List<CircleScript> circles = new List<CircleScript>();
    private List<Color> colors = new List<Color>();
    private int x;
    private int y;
    private float size = 1.1f;
    public float yOffset = 0.6f;
    private int randomColors;
    private Vector2 initialPos;
    // used to avoid repeating colors
    private int lastColor1;
    private int lastColor2;
    private int points;
    private TimeScript time;
    public GameObject circlePrefab;
    public GameObject pointsText;

    public GameObject gamePanel;
    public GameObject gameOverPanel;

    void Start()
    {
        initColors();
        time = FindObjectOfType<TimeScript>();
        points = 0;
        gamePanel.GetComponent<Animator>().SetBool("Open", true);
    }

    void initColors()
    {
        colors.Add(rgba(244, 67, 54)); //red
        colors.Add(rgba(156, 39, 176)); //purple
        // colors.Add(rgba(63, 81, 181)); // indigo
        colors.Add(rgba(33, 150, 243)); // blue
        colors.Add(rgba(76, 175, 80)); //green
        colors.Add(rgba(255, 235, 59)); //yellow
        colors.Add(rgba(255, 152, 0)); //orange
    }
    public void startGame()
    {
        inGame = true;
        //Init count of circles and position
        x = UnityEngine.Random.Range(2, 5);
        y = UnityEngine.Random.Range(2, 5);
        float middleCircle = (size / 2);
        float middleX = (x * size / 2) - middleCircle;
        float middleY = (y * size / 2) - middleCircle;
        initialPos = new Vector2(-middleX, -middleY - yOffset);
        // Init 2 colors
        int n1 = -1;
        int n2 = -1;
        while (true)
        {
            int i = UnityEngine.Random.Range(0, colors.Count - 1);
            if (i != lastColor1 && i != lastColor2)
            {
                if (n1 == -1)
                {
                    n1 = i;
                    lastColor1 = n1;
                }
                else
                {
                    n2 = i;
                    lastColor2 = n2;
                    break;
                }
            }
        }
        initCircles(n1, n2);
    }

    void initCircles(int color1, int color2)
    {
        int cantTotal = x * y;
        Debug.Log("Cantidad Total: " + cantTotal);
        int random1 = (cantTotal / 2) - 1;
        int random2 = (cantTotal / 2) + 1;
        Debug.Log("Random1: " + random1);
        Debug.Log("Random2: " + random2);
        randomColors = UnityEngine.Random.Range(random1, random2 + 1);

        List<Vector2> randomPos = getRandomPos(randomColors, x, y);
        Debug.Log("Cantidad de circulos secundarios: " + randomPos.Count);
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                Vector2 pos = new Vector2((i * size) + initialPos.x, (j * size) + initialPos.y);
                GameObject o = (GameObject)Instantiate(circlePrefab, pos, Quaternion.identity);
                CircleScript c = o.GetComponent<CircleScript>();
                Vector2 position = new Vector2(i, j);
                if (randomPos.Contains(position))
                {
                    c.setColors(colors[color1], colors[color2]);
                }
                else
                {
                    c.setColors(colors[color2], colors[color1]);
                }
                circles.Add(c);
            }
        }
    }

    List<Vector2> getRandomPos(int random, int x, int y)
    {
        List<Vector2> result = new List<Vector2>();
        while (random > 0)
        {
            int randomX = UnityEngine.Random.Range(0, x);
            int randomY = UnityEngine.Random.Range(0, y);
            Vector2 randomPos = new Vector2(randomX, randomY);
            if (!result.Contains(randomPos))
            {
                result.Add(randomPos);
                random--;
            }
        }
        return result;
    }

    void setColors(int color1, int color2, Circle c)
    {
        c.setColorA(color1);
        c.setColorB(color2);
    }

    static Color rgba(int r, int g, int b)
    {
        return new Color((float)r / 255, (float)g / 255, (float)b / 255, 0.66f);
    }

    public bool CheckWin(int solution)
    {
        bool win = sameColor();
        if (win)
        {
            points += solution;
            int minSolution = Mathf.Min(randomColors, circles.Count - randomColors);
            time.extraSeconds(minSolution, solution);
            pointsText.GetComponent<Text>().text = "SCORE: " + points.ToString();
            nextLevel();
        }
        return win;
    }

    public void nextLevel()
    {
        // flash ();
        foreach (CircleScript o in circles)
        {
            Destroy(o.gameObject);
        }
        circles = new List<CircleScript>();
        startGame();
    }

    private bool sameColor()
    {
        Color color = circles[0].getColor();
        for (int i = 1; i < circles.Count; i++)
        {
            if (color != circles[i].getColor())
            {
                return false;
            }
        }
        return true;
    }

    public void GameOver()
    {
        foreach (CircleScript o in circles)
        {
            Destroy(o.gameObject);
        }
        // Game Over
        gamePanel.GetComponent<Animator>().SetBool("Open", false);
        gameOverPanel.GetComponent<Animator>().SetBool("Open", true);
    }

}
