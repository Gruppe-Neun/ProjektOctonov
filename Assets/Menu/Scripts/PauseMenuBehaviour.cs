using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenuBehaviour : MonoBehaviour
{
    public static bool gameOver = false;
    public static bool gamePaused = false;
    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject lossUI;
    public SceneLoader sceneLoader;

    void Start()
    {
        pauseUI.SetActive(false);
        winUI.SetActive(false);
        lossUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!gameOver) {
            if (gamePaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void win() {
        gameOver = true;
        if (gamePaused) {
            pauseUI.SetActive(true);
            gamePaused = false;
        }
        Time.timeScale = 0f;
        winUI.SetActive(true);
    }

    public void loss() {
        gameOver = true;
        if (gamePaused) {
            pauseUI.SetActive(true);
            gamePaused = false;
        }
        Time.timeScale = 0f;
        lossUI.SetActive(true);
    }

    void Pause() {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void Resume() {
        pauseUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void Restart() {
        Time.timeScale = 1f;
        gamePaused = false;
        gameOver = false;
        sceneLoader.LoadScene(SceneManager.GetActiveScene().path);
    }

    public void Exit() {
        Time.timeScale = 1f;
        gamePaused = false;
        gameOver = false;
        sceneLoader.LoadScene("Assets/Scenes/Main Menu.unity");
    }
}
