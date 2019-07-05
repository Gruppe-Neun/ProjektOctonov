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
        gameObject.transform.position = new Vector3(180, 135, 0);
    }

    public void MoveToLvl2() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(479, 135, 0);
    }

    public void MoveToLvl3() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(773, 135, 0);
    }

    //TUTORIAL

    public void MoveToPreviousPage() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(803, 110, 0);
    }

    public void MoveToNextPage() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(903, 110, 0);
    }

    //OPTIONS

    public void MoveToToggle() {
        gameObject.SetActive(true);
        gameObject.transform.position = new Vector3(148, 277, 0);
    }

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
