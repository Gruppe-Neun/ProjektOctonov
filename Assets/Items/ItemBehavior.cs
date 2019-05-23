using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    public Item.Type type = Item.Type.UNDEF;
    public Item.useType useType = Item.useType.UNDEF;
    public int amount = 1;
    public float rotationSpeed = 20;

    public int status = 0; // 0 = undef, 1= in Inventory, 2 = dropped


    private float rotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (status == 2) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation += Time.deltaTime*rotationSpeed;
        transform.localRotation = Quaternion.AngleAxis(rotation,Vector3.down);
    }

    public void drop() {
        gameObject.SetActive(true);
        status = 2;
    }

    public void take() {
        gameObject.SetActive(false);
        status = 1;
    }

    public void OnTriggerEnter(Collider other) {
        
    }
}
