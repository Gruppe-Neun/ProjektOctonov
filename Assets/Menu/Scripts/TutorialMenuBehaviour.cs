using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMenuBehaviour : MonoBehaviour
{
    int[] pages;

    public GameObject mainMenu;

    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;

    void Start() {
        pages = new int[4];
    }

    void Update() {
    }

    public void OpenMainMenu() {
        mainMenu.SetActive(true);
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(false);
    }

    public void OpenPage1() {
        page1.SetActive(true);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(false);
    }

    public void OpenPage2() {
        page1.SetActive(false);
        page2.SetActive(true);
        page3.SetActive(false);
        page4.SetActive(false);
    }

    public void OpenPage3() {
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(true);
        page4.SetActive(false);
    }

    public void OpenPage4() {
        page1.SetActive(false);
        page2.SetActive(false);
        page3.SetActive(false);
        page4.SetActive(true);
    }
}
