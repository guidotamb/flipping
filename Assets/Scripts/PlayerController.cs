using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private int swapedCount = 0;

    private OnePlayerScript game;
    // Use this for initialization
    void Start()
    {
        game = FindObjectOfType<OnePlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartScene.singleton.loadScene("StartScene");
        }
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Circle")
                    {
                        CircleScript c = hit.collider.gameObject.GetComponent<CircleScript>();
                        swapedCount += c.AlreadySwapped() ? 0 : 1;
                        c.OnClicked();
                        if (game.CheckWin(swapedCount))
                        {
                            swapedCount = 0;
                        }
                    }
                }
            }
        }
    }
}
