using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private int scene;
    public void Resume()
    {
        scene = PlayerPrefs.GetInt("Level");
        SceneManager.LoadScene(scene);
        Time.timeScale = 1.0f;
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("Level", 1);
        scene = PlayerPrefs.GetInt("Level");
        SceneManager.LoadScene(scene);
    }

    public void Credits()
    {
        Debug.Log("This game hs developed by Reza Hashemi");
    }

    public void Exit()
    {
        Debug.Log("exited");
        Application.Quit();
    }
}
