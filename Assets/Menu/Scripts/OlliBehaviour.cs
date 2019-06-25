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

    void Update()
    {


        if (Time.time > time + 0.5f && buttonPressed) {
            gameObject.SetActive(false);
            gameObject.GetComponent<Animator>().SetBool("buttonPressed", false);
            buttonPressed = false;
        }
    }

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

    public void MoveToBack() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(827, 42, 0);
    }

    public void OnButtonPressed() {
        gameObject.GetComponent<Animator>().SetBool("buttonPressed", true);
        time = Time.time;
        buttonPressed = true;
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
