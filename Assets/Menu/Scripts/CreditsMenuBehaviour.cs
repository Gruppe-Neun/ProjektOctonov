using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject creditsMenu;

    void Start() {
    }

    void Update() {
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        creditsMenu.SetActive(false);
    }
}
