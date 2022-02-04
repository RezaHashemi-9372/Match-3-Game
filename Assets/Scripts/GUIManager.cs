using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private Text scoreTxt;
    [SerializeField]
    private Text recordTxt;
    [SerializeField]
    private Text moveTxt;

    private GameMode gameMode;
    private int scene;
    private void Awake()
    {
        gameOverPanel.SetActive(false);
        gameMode = FindObjectOfType<GameMode>();
        pausePanel.SetActive(false);

    }

    private void Update()
    {
        scoreTxt.text = string.Format("Score: {0}", GameMode.Score);
        recordTxt.text = string.Format("Record: {0}", GameMode.Record);
        moveTxt.text = string.Format("{0}", gameMode.Move);
    }
    #region PauseGame

    public void Pause()
    {
        Time.timeScale = 0.0f;
        pausePanel.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("It has exit");
        Application.Quit();
    }

    public void BackToGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    public void MainMenu()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(0);
    }
    #endregion

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Debug.Log("Game Over! try it again");
    }


}
