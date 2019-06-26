using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startGameMenu;

    void Start() {
    }

    void Update() {
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        startGameMenu.SetActive(false);
    }
}
