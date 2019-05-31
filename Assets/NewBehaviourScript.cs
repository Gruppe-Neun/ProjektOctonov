using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent(typeof(NavMeshBaker));
        GetComponent<NavMeshBaker>().buildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
