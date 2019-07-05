using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startGameMenu;
    public GameObject tutorialMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;

    void Start() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OpenMainMenu();
    }

    void Update() {
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        startGameMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void OpenStartGame() {
        mainMenu.SetActive(false);
        startGameMenu.SetActive(true);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void OpenTutorial() {
        mainMenu.SetActive(false);
        startGameMenu.SetActive(false);
        tutorialMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void OpenOptions() {
        mainMenu.SetActive(false);
        startGameMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }

    public void OpenCredits() {
        mainMenu.SetActive(false);
        startGameMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
