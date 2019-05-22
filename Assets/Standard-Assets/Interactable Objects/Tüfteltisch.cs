using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tüfteltisch : MonoBehaviour, IInteractable {

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Interact() {
        Debug.Log("Interacted with " + gameObject.ToString());
    }
}