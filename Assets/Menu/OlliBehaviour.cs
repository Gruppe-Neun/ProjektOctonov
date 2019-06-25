using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OlliBehaviour : MonoBehaviour
{
    public Button startGame;
    public Button tutorial;
    public Button options;
    public Button credits;
    
    void Start()
    {
        gameObject.transform.position = new Vector3(638, 346, 0);
    }

    void Update()
    {
    }

    public void MoveToStartGame() {
        gameObject.transform.position = new Vector3(638, 346, 0);
    }

    public void MoveToTutorial() {
        gameObject.transform.position = new Vector3(638, 269, 0);
    }

    public void MoveToOptions() {
        gameObject.transform.position = new Vector3(638, 192, 0);
    }

    public void MoveToCredits() {
        gameObject.transform.position = new Vector3(638, 121, 0);
    }
}
