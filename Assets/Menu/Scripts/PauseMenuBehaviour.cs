using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class PauseMenuBehaviour : MonoBehaviour
{
    public static bool gamePaused = false;
    public GameObject pauseUI;
    public SceneLoader sceneLoader;

    void Start()
    {
        pauseUI.SetActive(false);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (gamePaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
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
        gamePaused = false;
        sceneLoader.LoadScene(SceneManager.GetActiveScene().path);
    }

    public void Exit() {
        gamePaused = false;
        sceneLoader.LoadScene("Assets/Scenes/Main Menu.unity");
    }
}
