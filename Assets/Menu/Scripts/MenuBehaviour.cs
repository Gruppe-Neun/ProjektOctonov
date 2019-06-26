using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startGameMenu;
    //private GameObject tutorialMenu;
    //private GameObject optionsMenu;
    //private GameObject creditsMenu;

    void Start() {
        //mainMenu =  GameObject.FindGameObjectWithTag("MainMenu");
        //startGameMenu = GameObject.FindGameObjectWithTag("StartGameMenu");
        //tutorialMenu = GameObject.FindGameObjectWithTag("TutorialMenu");
        //optionsMenu = GameObject.FindGameObjectWithTag("OptionsMenu");
        //creditsMenu = GameObject.FindGameObjectWithTag("CreditsMenu");

        OpenMainMenu();
    }
    
    void Update() {
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        startGameMenu.SetActive(false);
        //tutorialMenu.SetActive(false);
        //optionsMenu.SetActive(false);
        //creditsMenu.SetActive(false);
    }

    public void OpenStartGame() {
        mainMenu.SetActive(false);
        startGameMenu.SetActive(true);
        //tutorialMenu.SetActive(false);
        //optionsMenu.SetActive(false);
        //creditsMenu.SetActive(false);
    }

    //public void OpenTutorial() {
    //    mainMenu.SetActive(false);
    //    startGameMenu.SetActive(false);
    //    tutorialMenu.SetActive(true);
    //    optionsMenu.SetActive(false);
    //    creditsMenu.SetActive(false);
    //}

    //public void OpenOptions() {
    //    mainMenu.SetActive(false);
    //    startGameMenu.SetActive(false);
    //    tutorialMenu.SetActive(false);
    //    optionsMenu.SetActive(true);
    //    creditsMenu.SetActive(false);
    //}

    //public void OpenCredits() {
    //    mainMenu.SetActive(false);
    //    startGameMenu.SetActive(false);
    //    tutorialMenu.SetActive(false);
    //    optionsMenu.SetActive(false);
    //    creditsMenu.SetActive(true);
    //}
}
