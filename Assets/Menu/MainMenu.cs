using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start() {
        
    }
    
    void Update() {
        
    }

    public void StartGameClick() {
        SceneManager.LoadScene("Assets/Scenes/Main.unity");
    }

    public void TutorialClick() {

    }

    public void OptionsClick() {

    }

    public void CreditsClick() {

    }
}
