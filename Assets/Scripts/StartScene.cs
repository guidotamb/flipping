using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartScene : MonoBehaviour
{
    public static StartScene singleton;

    public GameObject menu;
    public GameObject onePlayerMenu;
    void Start()
    {
        singleton = this;
        menu.GetComponent<Animator>().SetBool("Open", true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void play()
    {
        menu.GetComponent<Animator>().SetBool("Open", false);
        onePlayerMenu.GetComponent<Animator>().SetBool("Open", true);
    }

    public void startGame(string difficulty)
    {
        PlayerPrefs.SetString("Difficulty", difficulty);
        loadScene("OnePlayer");
    }

    public void play2()
    {
        // loadScene("TwoPlayer");
    }

    public void loadScene(string sceneName)
    {
        StartCoroutine(ChangeScene(sceneName));
    }
    IEnumerator ChangeScene(string SceneName)
    {
        float fadeTime = GetComponent<FadingScenes>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(SceneName);
    }
}
