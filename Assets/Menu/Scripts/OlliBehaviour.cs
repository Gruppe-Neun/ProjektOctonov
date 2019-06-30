using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OlliBehaviour : MonoBehaviour
{
    private float time = 0;
    private bool buttonPressed;

    void Start()
    {
        gameObject.SetActive(false);
        buttonPressed = false;
    }

    //void Update()
    //{
    //    if (Time.time > time + 0.1f && buttonPressed) {
    //        gameObject.SetActive(false);
    //        gameObject.GetComponent<Animator>().SetBool("buttonPressed", false);
    //        buttonPressed = false;
    //    }
    //}

    //MAIN

    public void MoveToStartGame() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(638, 334, 0);
    }

    public void MoveToTutorial() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(638, 267, 0);
    }

    public void MoveToOptions() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(638, 201, 0);
    }

    public void MoveToCredits() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(638, 139, 0);
    }

    //STARTGAME

    public void MoveToLvl1() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(179.8f, 135, 0);
    }

    public void MoveToLvl2() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(479.1f, 135, 0);
    }

    public void MoveToLvl3() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(773.4f, 135, 0);
    }

    //TUTORIAL

    //OPTIONS

    //CREDITS

    //BACK

    public void MoveToBack() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(827, 42, 0);
    }

    public void OnButtonPressed() {
        //gameObject.GetComponent<Animator>().SetBool("buttonPressed", true);
        //time = Time.time;
        //buttonPressed = true;
        gameObject.SetActive(false);
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
