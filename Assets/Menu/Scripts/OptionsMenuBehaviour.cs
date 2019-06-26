using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;

    void Start() {
    }

    void Update() {
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
