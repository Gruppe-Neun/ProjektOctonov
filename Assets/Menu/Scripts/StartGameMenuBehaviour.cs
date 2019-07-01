using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameMenuBehaviour : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject startGameMenu;
    public SceneLoader sceneLoader;

    void Start() {
    }

    void Update() {
    }

    public void StartLvl1() {
        gameObject.SetActive(false);
        sceneLoader.LoadScene("Assets/Scenes/Level1.unity");
    }

    public void StartLvl2() {
        gameObject.SetActive(false);
        sceneLoader.LoadScene("Assets/Scenes/Level2.unity");
    }

    public void StartLvl3() {
        gameObject.SetActive(false);
        sceneLoader.LoadScene("Assets/Scenes/Level3.unity");
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        startGameMenu.SetActive(false);
    }
}
