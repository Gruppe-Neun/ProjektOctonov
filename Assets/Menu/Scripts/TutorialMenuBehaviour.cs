using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject tutorialMenu;

    void Start() {
    }

    void Update() {
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        tutorialMenu.SetActive(false);
    }
}
